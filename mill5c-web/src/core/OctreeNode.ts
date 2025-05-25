import * as THREE from 'three';
import { Point3D } from './Geometry';
import { NodeColor } from './NodeColor';
import { Octree } from './Octree'; // Forward declaration for parent reference

export class OctreeNode {
    public center: Point3D;
    public halfSize: number; // L in C#
    public color: NodeColor;
    public children: OctreeNode[] | null = null;
    public parentOctree: Octree; // Reference to the parent Octree
    public parentNode: OctreeNode | null = null; // Reference to parent node

    // Bounding sphere radius (R in C#), L * sqrt(3)
    public boundingSphereRadius: number;

    constructor(parentOctree: Octree, center: Point3D, halfSize: number, parentNode: OctreeNode | null = null) {
        this.parentOctree = parentOctree;
        this.center = center;
        this.halfSize = halfSize;
        this.color = NodeColor.Solid; // Default to Solid (Black)
        this.parentNode = parentNode;
        this.boundingSphereRadius = this.halfSize * Math.sqrt(3);
    }

    public split(): void {
        if (this.children !== null) {
            // Already split
            return;
        }

        // Cannot split further if halfSize is already at or below minHalfSize
        if (this.halfSize <= this.parentOctree.minHalfSize) {
            return;
        }

        this.children = [];
        const childHalfSize = this.halfSize * 0.5;

        // Check if childHalfSize would be too small BEFORE creating children
        if (childHalfSize < this.parentOctree.minHalfSize) {
            // If splitting would result in nodes smaller than allowed,
            // do not split and potentially mark this node as indivisible or handle accordingly.
            // For now, we just prevent the split.
            this.children = null; // Ensure children remains null
            return;
        }

        for (let i = 0; i < 2; i++) { // Corresponds to x-axis
            for (let j = 0; j < 2; j++) { // Corresponds to y-axis
                for (let k = 0; k < 2; k++) { // Corresponds to z-axis
                    const childCenter = new THREE.Vector3(
                        this.center.x + (i === 0 ? -childHalfSize : childHalfSize),
                        this.center.y + (j === 0 ? -childHalfSize : childHalfSize),
                        this.center.z + (k === 0 ? -childHalfSize : childHalfSize)
                    );
                    const childNode = new OctreeNode(this.parentOctree, childCenter, childHalfSize, this);
                    this.children.push(childNode);
                }
            }
        }
        
        // Optional: Notify parentOctree about the split if needed (similar to C# event)
        // this.parentOctree.onNodeSplit(this);
    }

    // Helper to get child by index (0-7), similar to C# this[int index]
    public getChild(index: number): OctreeNode | undefined {
        return this.children?.[index];
    }
}
