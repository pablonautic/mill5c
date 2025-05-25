import * as THREE from 'three';
import { Point3D, Vector3D } from './Geometry';

export interface PathPoint {
    position: Point3D;
    orientation: Vector3D;
}

export const DEFAULT_ORIENTATION: Readonly<Vector3D> = new THREE.Vector3(0, 0, 1); // Z-up, matching C# Vector3D.Up

export function createDefaultPathPoint(): PathPoint {
    return {
        position: new THREE.Vector3(0, 0, 0),
        orientation: DEFAULT_ORIENTATION.clone()
    };
}
