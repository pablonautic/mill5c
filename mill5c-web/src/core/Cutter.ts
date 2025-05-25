import * as THREE from 'three';
import { Point3D, Vector3D } from './Geometry';

export enum CutterType {
    Ball = 'ball',
    Flat = 'flat'
}

export interface Cutter {
    position: Point3D;
    orientation: Vector3D;
    radius: number;
    height: number;
    type: CutterType;
    id?: number; // Optional ID, similar to C#
}

export function createDefaultCutter(
    type: CutterType = CutterType.Flat,
    radius: number = 1,
    height: number = 5 
): Cutter {
    return {
        position: new THREE.Vector3(0, 0, 0), // Default position
        orientation: new THREE.Vector3(0, 0, 1), // Default Z-up
        radius,
        height,
        type
    };
}
