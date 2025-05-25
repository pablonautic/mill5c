import * as THREE from 'three';
import { PathPoint } from './PathPoint';
import { Cutter } from './Cutter'; 

export class PathInterpolator {
    public pathPoints: PathPoint[] = [];
    public positionEpsilon: number; 
    public orientationEpsilon: number; 

    private currentSegmentIndex: number = -1;
    private tSegment: number = 0; 
    private stepsInSegment: number = 0;
    private currentStepInSegment: number = 0;
    
    public isFinished: boolean = true;

    // Helper for Slerp: Default vector from which orientations are calculated
    private static readonly DEFAULT_QUATERNION_UP = new THREE.Vector3(0, 0, 1);

    constructor(
        pathPoints: PathPoint[],
        positionEpsilon: number = 0.1, 
        orientationEpsilon: number = 0.01 
    ) {
        this.pathPoints = pathPoints;
        this.positionEpsilon = positionEpsilon;
        this.orientationEpsilon = orientationEpsilon;
        this.reset();
    }

    public reset(): void {
        this.currentSegmentIndex = -1;
        this.tSegment = 0;
        this.stepsInSegment = 0;
        this.currentStepInSegment = 0;
        this.isFinished = (this.pathPoints.length < 2); 
        if (!this.isFinished) {
            this.prepareNextSegment();
        }
    }

    private prepareNextSegment(): void {
        this.currentSegmentIndex++;
        if (this.currentSegmentIndex + 1 >= this.pathPoints.length) {
            this.isFinished = true;
            return;
        }

        const p1 = this.pathPoints[this.currentSegmentIndex].position;
        const p2 = this.pathPoints[this.currentSegmentIndex + 1].position;
        const o1 = this.pathPoints[this.currentSegmentIndex].orientation;
        const o2 = this.pathPoints[this.currentSegmentIndex + 1].orientation;

        const distance = p1.distanceTo(p2);
        const angle = o1.angleTo(o2); 

        const numPositionSteps = (this.positionEpsilon > 0 && distance > 0) ? Math.max(1, Math.floor(distance / this.positionEpsilon)) : 1;
        const numOrientationSteps = (this.orientationEpsilon > 0 && angle > 0) ? Math.max(1, Math.floor(angle / this.orientationEpsilon)) : 1;
        
        this.stepsInSegment = Math.max(numPositionSteps, numOrientationSteps, 1); 
        this.currentStepInSegment = 0;
        this.tSegment = 0; 
    }

    public step(cutter: Cutter): boolean {
        if (this.isFinished) {
            return false;
        }

        this.currentStepInSegment++;
        if (this.currentStepInSegment > this.stepsInSegment) {
            this.prepareNextSegment();
            if (this.isFinished) {
                // Ensure cutter is exactly at the last point if pathPoints is not empty
                if (this.pathPoints.length > 0) {
                    const lastPoint = this.pathPoints[this.pathPoints.length - 1];
                    cutter.position.copy(lastPoint.position);
                    cutter.orientation.copy(lastPoint.orientation).normalize();
                }
                return false;
            }
        }
        
        this.tSegment = this.stepsInSegment > 0 ? this.currentStepInSegment / this.stepsInSegment : 1.0;
        this.tSegment = Math.min(this.tSegment, 1.0); 

        const currentP = this.pathPoints[this.currentSegmentIndex];
        const nextP = this.pathPoints[this.currentSegmentIndex + 1];

        cutter.position.lerpVectors(currentP.position, nextP.position, this.tSegment);
        
        // Orientation Slerp
        const q1 = new THREE.Quaternion().setFromUnitVectors(PathInterpolator.DEFAULT_QUATERNION_UP, currentP.orientation.clone().normalize());
        const q2 = new THREE.Quaternion().setFromUnitVectors(PathInterpolator.DEFAULT_QUATERNION_UP, nextP.orientation.clone().normalize());
        const qt = new THREE.Quaternion();
        
        THREE.Quaternion.slerp(q1, q2, qt, this.tSegment);
        cutter.orientation.copy(PathInterpolator.DEFAULT_QUATERNION_UP).applyQuaternion(qt).normalize();

        return true;
    }

    public interpolateTo(cutter: Cutter, tGlobal: number): void {
        if (this.pathPoints.length === 0) return;
        if (this.pathPoints.length === 1) {
             cutter.position.copy(this.pathPoints[0].position);
             cutter.orientation.copy(this.pathPoints[0].orientation).normalize();
             return;
        }

        tGlobal = Math.max(0, Math.min(1, tGlobal)); 
        const totalSegments = this.pathPoints.length - 1;
        const globalPointProgress = tGlobal * totalSegments;
        const segmentIdx = Math.floor(globalPointProgress);
        const tSeg = globalPointProgress - segmentIdx;

        // Ensure segmentIdx does not exceed bounds if tGlobal is exactly 1
        const p1Index = Math.min(segmentIdx, this.pathPoints.length - 2);
        const p2Index = Math.min(segmentIdx + 1, this.pathPoints.length - 1);


        const p1 = this.pathPoints[p1Index];
        const p2 = this.pathPoints[p2Index];

        cutter.position.lerpVectors(p1.position, p2.position, tSeg);
        
        const q1 = new THREE.Quaternion().setFromUnitVectors(PathInterpolator.DEFAULT_QUATERNION_UP, p1.orientation.clone().normalize());
        const q2 = new THREE.Quaternion().setFromUnitVectors(PathInterpolator.DEFAULT_QUATERNION_UP, p2.orientation.clone().normalize());
        const qt = new THREE.Quaternion();
        THREE.Quaternion.slerp(q1, q2, qt, tSeg);
        cutter.orientation.copy(PathInterpolator.DEFAULT_QUATERNION_UP).applyQuaternion(qt).normalize();
    }
}
