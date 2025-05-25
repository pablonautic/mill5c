import { Octree } from './Octree';
import { OctreeNode } from './OctreeNode';
import { Cutter, CutterType } from './Cutter'; // Added CutterType
import { CollisionHelper, CollisionType } from './CollisionHelper';
import { NodeColor } from './NodeColor';

export class OctreeMaterial {
    public octree: Octree;
    // Eps in C# was min node half size, stored in Octree.ts as minHalfSize
    // public minNodeHalfSize: number; // Already available via this.octree.minHalfSize

    // Statistics (optional, but good for debugging)
    public maxRecurrenceDepthReached: number = 0;

    constructor(octree: Octree) {
        this.octree = octree;
        // this.minNodeHalfSize = octree.minHalfSize; // Store if frequently accessed
    }

    /**
     * Intersects the entire material (octree) with the cutter.
     * Starts evaluation from the root node.
     */
    public intersect(cutter: Cutter): void {
        this.maxRecurrenceDepthReached = 0; // Reset for this intersection pass
        if (this.octree.rootNode) { // Ensure rootNode exists
            this.evaluateNode(this.octree.rootNode, cutter, 0);
        }
    }

    /**
     * Recursively evaluates a node-cutter collision.
     * Based on C# OctreeMaterial.EvalNode.
     */
    private evaluateNode(node: OctreeNode, cutter: Cutter, depth: number): void {
        if (depth > this.maxRecurrenceDepthReached) {
            this.maxRecurrenceDepthReached = depth;
        }

        // Optimization: If node is already empty, no further processing needed.
        if (node.color === NodeColor.Empty) {
            return;
        }

        // Determine collision type between cutter and node's bounding sphere
        let collision: CollisionType;
        
        if (cutter.type === CutterType.Ball) {
            const sphereCol = CollisionHelper.sphereSphere(cutter.position, cutter.radius, node.center, node.boundingSphereRadius);
            // For ball cutter, its cylinder part's base is effectively its position (tip) for this check
            const cylinderCol = CollisionHelper.cylinderSphere(node.center, node.boundingSphereRadius, cutter.position, cutter.orientation, cutter.radius, cutter.height);
            
            if (sphereCol === CollisionType.Total || cylinderCol === CollisionType.Total) {
                collision = CollisionType.Total;
            } else if (sphereCol === CollisionType.Partial || cylinderCol === CollisionType.Partial) {
                collision = CollisionType.Partial;
            } else {
                collision = CollisionType.None;
            }
        } else { // CutterType.Flat
            collision = CollisionHelper.cylinderSphere(node.center, node.boundingSphereRadius, cutter.position, cutter.orientation, cutter.radius, cutter.height);
        }


        switch (collision) {
            case CollisionType.None:
                // No collision, node remains as it is (could be Solid or already Empty/Partial from previous cuts)
                return;
            case CollisionType.Total:
                node.color = NodeColor.Empty; // Node is completely removed
                node.children = null; // Remove children if any, as parent is now empty
                return; // No need to evaluate children if parent is totally removed
            case CollisionType.Partial:
                node.color = NodeColor.Partial; // Mark as partially affected
                // If it was Solid, it becomes Partial. If it was already Partial, it stays Partial.
                // If it was Empty (though we check this at the start), this would incorrectly make it Partial.
                // However, the initial check for NodeColor.Empty should prevent this.
                break;
        }

        // If node is smaller than or equal to the minimum size, stop recursion (it remains Partial)
        if (node.halfSize <= this.octree.minHalfSize) {
            // Optional: Raise an event like NodeEpsReached if needed
            return;
        }

        // If collision is Partial and node is large enough, split if not already split
        if (node.children === null) {
            node.split();
            // If node.split() failed (e.g., would make children too small), node.children remains null.
        }

        // Recursively evaluate children if the node was split (or already had children)
        let allChildrenEmpty = true;
        if (node.children) {
            for (const child of node.children) {
                // Optimization: only evaluate children that haven't already been fully carved out.
                // This is crucial because evaluateNode sets already-empty children to remain empty.
                if (child.color !== NodeColor.Empty) { 
                   this.evaluateNode(child, cutter, depth + 1);
                }
                // Check color *after* evaluation
                if (child.color !== NodeColor.Empty) {
                    allChildrenEmpty = false;
                }
            }
        } else {
            // If there are no children (e.g. split failed or node was too small to split but was partial),
            // then it's not "all children empty", so the node itself (being partial) is not empty.
            allChildrenEmpty = false; 
        }

        // If all children are now empty, this node also becomes empty
        if (allChildrenEmpty) {
            node.color = NodeColor.Empty;
            node.children = null; // Consolidate children
        }
    }

    public reset(): void {
        // This would typically involve resetting the octree itself
        // The C# version did Tree.Reset(), which created a new root node.
        // Assuming Octree.reset() handles this.
        if (this.octree && this.octree.rootNode) { // Check if octree and rootNode exist
           this.octree.reset(this.octree.rootNode.center, this.octree.rootNode.halfSize * 2);
        }
        this.maxRecurrenceDepthReached = 0;
    }
}
