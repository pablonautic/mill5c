import * as THREE from 'three';
import { Point3D } from './Geometry';
import { OctreeNode } from './OctreeNode';

export class Octree {
    public rootNode: OctreeNode;
    public minHalfSize: number = 0.1; // Default minimum node half size (Eps in C#)

    constructor(center: Point3D, size: number, minNodeHalfSize: number = 0.1) {
        this.minHalfSize = minNodeHalfSize;
        // In C#, Octree constructor took L (halfSize), here 'size' is full initial cube size
        // The OctreeNode constructor expects the parentOctree as the first argument.
        this.rootNode = new OctreeNode(this, center, size * 0.5);
    }

    public reset(center: Point3D, size: number): void {
         this.rootNode = new OctreeNode(this, center, size * 0.5);
         // Consider garbage collection implications if needed, though JS is different from C#
    }

    // Optional: If event-like behavior for node splitting is needed
    // public onNodeSplit(node: OctreeNode): void {
    //     console.log('Node split:', node);
    // }
}
