import * as THREE from 'three';

export type Point3D = THREE.Vector3;
export type Vector3D = THREE.Vector3;

// Helper function (optional, but good for consistency with C#)
export function distanceSq(p1: Point3D, p2: Point3D): number {
    return p1.distanceToSquared(p2);
}

export function distance(p1: Point3D, p2: Point3D): number {
    return p1.distanceTo(p2);
}
