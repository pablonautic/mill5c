import * as THREE from 'three';
import { Octree } from '../core/Octree';
import { OctreeNode } from '../core/OctreeNode';
import { NodeColor } from '../core/NodeColor';

export class OctreeVisualizer {
    private scene: THREE.Scene;
    private octreeMeshGroup: THREE.Group; // Group to hold all octree node meshes

    // Define materials for different node colors
    private solidMaterial = new THREE.MeshLambertMaterial({ color: 0x0077ff }); // Blue
    private partialMaterial = new THREE.MeshLambertMaterial({ color: 0xffdd00, transparent: true, opacity: 0.7 }); // Yellow, semi-transparent

    constructor(scene: THREE.Scene) {
        this.scene = scene;
        this.octreeMeshGroup = new THREE.Group();
        this.scene.add(this.octreeMeshGroup);
    }

    public visualize(octree: Octree): void {
        this.clear(); // Clear previous visualization

        if (octree && octree.rootNode) {
            this.visualizeNode(octree.rootNode);
        }
    }

    private visualizeNode(node: OctreeNode): void {
        if (node.color === NodeColor.Empty) {
            return; // Don't render empty nodes
        }

        if (node.children && node.children.length > 0) {
            // This is a split node (originally gray but not a leaf), recurse
            for (const child of node.children) {
                this.visualizeNode(child);
            }
        } else {
            // This is a leaf node (or an unsplit node)
            // Only render Solid or Partial leaf nodes
            const cubeGeometry = new THREE.BoxGeometry(node.halfSize * 2, node.halfSize * 2, node.halfSize * 2);
            let material;

            if (node.color === NodeColor.Solid) {
                material = this.solidMaterial;
            } else if (node.color === NodeColor.Partial) {
                material = this.partialMaterial;
            } else {
                // Should not happen if Empty nodes are skipped, but as a fallback:
                cubeGeometry.dispose(); // Dispose unused geometry
                return;
            }

            const cubeMesh = new THREE.Mesh(cubeGeometry, material);
            cubeMesh.position.copy(node.center);
            this.octreeMeshGroup.add(cubeMesh);
        }
    }

    public clear(): void {
        // Remove all children from the group and dispose of their geometry/material
        while (this.octreeMeshGroup.children.length > 0) {
            const child = this.octreeMeshGroup.children[0] as THREE.Mesh;
            this.octreeMeshGroup.remove(child);
            child.geometry.dispose();
            // Materials are shared, dispose them elsewhere if they are dynamically created per node
        }
    }
    
    public disposeMaterials(): void {
        this.solidMaterial.dispose();
        this.partialMaterial.dispose();
    }
}
