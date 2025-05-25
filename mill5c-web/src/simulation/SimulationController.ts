import * as THREE from 'three';
import { Octree } from '../core/Octree';
import { OctreeMaterial } from '../core/OctreeMaterial';
import { Cutter, createDefaultCutter, CutterType } from '../core/Cutter';
import { PathPoint } from '../core/PathPoint';
import { PathInterpolator } from '../core/PathInterpolator';
import { OctreeVisualizer } from '../visualization/OctreeVisualizer';
import { CutterVisualizer } from '../visualization/CutterVisualizer';
import { Point3D } from '../core/Geometry'; // Assuming Point3D is THREE.Vector3

export enum SimulationState {
    Idle,      // Not started, or after reset
    Running,
    Paused,
    Finished
}

export class SimulationController {
    public octree: Octree;
    public octreeMaterial: OctreeMaterial;
    public cutter: Cutter;
    public pathPoints: PathPoint[] = [];
    public pathInterpolator?: PathInterpolator;

    public octreeVisualizer: OctreeVisualizer;
    public cutterVisualizer: CutterVisualizer;

    public state: SimulationState = SimulationState.Idle;
    private animationFrameId?: number;

    // Simulation parameters
    public stepsPerFrame: number = 1; // How many interpolation steps per animation frame

    constructor(
        scene: THREE.Scene,
        initialOctreeCenter: Point3D,
        initialOctreeSize: number,
        initialMinNodeHalfSize: number = 0.1
    ) {
        this.octree = new Octree(initialOctreeCenter, initialOctreeSize, initialMinNodeHalfSize);
        this.octreeMaterial = new OctreeMaterial(this.octree);
        // Default cutter, will be updated when path is loaded
        this.cutter = createDefaultCutter(CutterType.Flat, 1, 5); 

        this.octreeVisualizer = new OctreeVisualizer(scene);
        this.cutterVisualizer = new CutterVisualizer(scene);

        this.visualizeCurrentState(); // Initial visualization
    }

    public loadPath(path: PathPoint[], cutterRadius: number = 1, cutterHeight: number = 5, cutterType: CutterType = CutterType.Flat): void {
        if (this.state === SimulationState.Running) {
            this.pause();
        }
        this.pathPoints = path;
        if (this.pathPoints.length > 0) {
            this.pathInterpolator = new PathInterpolator(this.pathPoints);
            this.cutter = createDefaultCutter(cutterType, cutterRadius, cutterHeight);
            // Set cutter to the start of the path
            this.pathInterpolator.interpolateTo(this.cutter, 0); 
        } else {
            this.pathInterpolator = undefined;
        }
        this.resetSimulationState(); // Resets octree and visualizers
        console.log('Path loaded. Points:', this.pathPoints.length);
    }
    
    private resetSimulationState(): void {
        this.octreeMaterial.reset(); // Resets octree to initial state
        if (this.pathInterpolator) {
            this.pathInterpolator.reset();
            // Position cutter at the start of the path
            this.pathInterpolator.interpolateTo(this.cutter, 0);
        } else {
            // Place cutter at a default position if no path
             this.cutter.position.set(this.octree.rootNode.center.x + this.octree.rootNode.halfSize + this.cutter.radius + 1, 
                                     this.octree.rootNode.center.y, 
                                     this.octree.rootNode.center.z);
        }
        this.state = SimulationState.Idle;
        this.visualizeCurrentState();
    }

    public start(): void {
        if (this.state === SimulationState.Running || !this.pathInterpolator || this.pathInterpolator.isFinished) {
            return;
        }
        this.state = SimulationState.Running;
        this.gameLoop();
        console.log('Simulation started.');
    }

    public pause(): void {
        if (this.state !== SimulationState.Running) {
            return;
        }
        this.state = SimulationState.Paused;
        if (this.animationFrameId) {
            cancelAnimationFrame(this.animationFrameId);
            this.animationFrameId = undefined;
        }
        console.log('Simulation paused.');
    }

    public resume(): void {
        if (this.state !== SimulationState.Paused) {
            return;
        }
        this.state = SimulationState.Running;
        this.gameLoop();
        console.log('Simulation resumed.');
    }
    
    public stepOnce(): void {
         if (this.state === SimulationState.Running) this.pause();
         if (!this.pathInterpolator || this.pathInterpolator.isFinished) return;

         const moved = this.pathInterpolator.step(this.cutter);
         if (moved) {
             this.octreeMaterial.intersect(this.cutter);
             this.visualizeCurrentState();
         } else {
             this.state = SimulationState.Finished;
             console.log('Simulation finished (manual step).');
         }
    }

    private gameLoop = (): void => { // Use arrow function to preserve 'this'
        if (this.state !== SimulationState.Running) {
            return;
        }

        for (let i = 0; i < this.stepsPerFrame; i++) {
            if (!this.pathInterpolator || this.pathInterpolator.isFinished) {
                this.state = SimulationState.Finished;
                console.log('Simulation finished.');
                this.visualizeCurrentState(); // Final update of visuals
                return;
            }
            const moved = this.pathInterpolator.step(this.cutter);
            if (moved) {
                this.octreeMaterial.intersect(this.cutter);
            } else {
                 // Should be caught by isFinished check above, but as a safeguard
                this.state = SimulationState.Finished;
                console.log('Simulation finished (interpolator reported no move).');
                this.visualizeCurrentState();
                return;
            }
        }
        
        this.visualizeCurrentState();
        this.animationFrameId = requestAnimationFrame(this.gameLoop);
    }

    private visualizeCurrentState(): void {
        this.octreeVisualizer.visualize(this.octree); // Re-visualize the octree
        this.cutterVisualizer.update(this.cutter);   // Update cutter's visual position/orientation
    }

    public dispose(): void {
        this.pause(); // Stop animation loop
        this.octreeVisualizer.disposeMaterials();
        this.cutterVisualizer.disposeMaterials();
        // Any other cleanup
    }
}
