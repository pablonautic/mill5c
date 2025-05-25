import * as THREE from 'three';
import { Point3D, Vector3D } from './Geometry'; // Assuming these are type aliases for THREE.Vector3

export enum CollisionType {
    None,
    Partial,
    Total
}

export class CollisionHelper {

    /**
     * Intersects two spheres. Note: In the C# version, this was not commutative
     * (cutterSphere vs otherSphere). We'll maintain that by convention here:
     * sphere1 is the "cutter" or "active" sphere.
     * sphere2 is the "passive" or "octree node" sphere.
     */
    public static sphereSphere(
        sphere1Center: Point3D,
        sphere1Radius: number,
        sphere2Center: Point3D,
        sphere2Radius: number
    ): CollisionType {
        const distSq = sphere1Center.distanceToSquared(sphere2Center);
        const sumRadii = sphere1Radius + sphere2Radius;

        if (distSq > sumRadii * sumRadii) {
            return CollisionType.None;
        } else {
            // Collision for sure
            const dist = Math.sqrt(distSq);
            if (dist + sphere2Radius < sphere1Radius) { // sphere2 is inside sphere1
                return CollisionType.Total;
            } else {
                return CollisionType.Partial;
            }
        }
    }

    /**
     * Intersects a cylinder and a sphere.
     */
    public static cylinderSphere(
        sphereCenter: Point3D,
        sphereRadius: number,
        cylinderBasePosition: Point3D, // Bottom center of the cylinder
        cylinderOrientation: Vector3D, // Normalized axis of the cylinder
        cylinderRadius: number,
        cylinderHeight: number
    ): CollisionType {
        const cylinderAxis = cylinderOrientation.clone().normalize(); // Ensure normalized

        // Vector from cylinder base to sphere center
        const vecCylBaseToSphereCenter = new THREE.Vector3().subVectors(sphereCenter, cylinderBasePosition);

        // Projection of vecCylBaseToSphereCenter onto the cylinder axis
        // This gives the distance along the axis from the base to the projection point
        const projectionDistance = vecCylBaseToSphereCenter.dot(cylinderAxis);

        // The closest point on the cylinder's infinite axis to the sphere's center
        const closestPointOnAxis = new THREE.Vector3()
            .copy(cylinderBasePosition)
            .addScaledVector(cylinderAxis, projectionDistance);

        // Distance squared from sphere center to its projection on the cylinder axis
        const distSphereCenterToAxisSq = sphereCenter.distanceToSquared(closestPointOnAxis);

        // Check 1: Sphere is too far away from the cylinder's infinite axis
        if (distSphereCenterToAxisSq > (cylinderRadius + sphereRadius) * (cylinderRadius + sphereRadius)) {
            return CollisionType.None;
        }

        // Check 2: Sphere is too far above or below the cylinder's height extents
        // (i.e., its projection falls outside the cylinder's height segment on the axis)
        if (projectionDistance < -sphereRadius || projectionDistance > cylinderHeight + sphereRadius) {
             // This checks if the sphere is beyond the caps including its own radius
            return CollisionType.None;
        }
        
        // Check 3: Sphere's projection on axis is within cylinder height, 
        // and sphere is close enough to axis for potential collision.
        
        if (distSphereCenterToAxisSq > cylinderRadius * cylinderRadius) {
            // Sphere center is outside the cylinder's radius.
            // Potential collision is with the cylinder's "corner" (edge of the cap).
            
            // Determine the center of the cap closest to the sphere's axial projection.
            let capCenter: Point3D;
            if (projectionDistance < 0) { // Closest to bottom cap
                capCenter = cylinderBasePosition.clone();
            } else if (projectionDistance > cylinderHeight) { // Closest to top cap
                capCenter = new THREE.Vector3().copy(cylinderBasePosition).addScaledVector(cylinderAxis, cylinderHeight);
            } else {
                // This path means:
                // 1. distSphereCenterToAxisSq > cylinderRadius * cylinderRadius (sphere center is radially outside the cylinder shaft)
                // 2. projectionDistance is BETWEEN 0 and cylinderHeight (sphere center's projection is along the shaft, not beyond caps)
                // This is a side collision with the cylinder's curved surface.
                // The closest point on cylinder's surface is on its side.
                // Distance from sphere center to axis is sqrt(distSphereCenterToAxisSq).
                // Distance from sphere center to cylinder surface is sqrt(distSphereCenterToAxisSq) - cylinderRadius.
                if (Math.sqrt(distSphereCenterToAxisSq) < cylinderRadius + sphereRadius) {
                    return CollisionType.Partial;
                }
                return CollisionType.None;
            }
            
            // For cases where projection is beyond caps (handled by capCenter assignment above):
            // Find the point on the cap's circular edge closest to the sphere's center.
            // Vector from cap center to sphere center:
            const vecCapCenterToSphereCenter = new THREE.Vector3().subVectors(sphereCenter, capCenter);
            // Project this vector onto the cap plane (which is perpendicular to cylinderAxis)
            // by removing the component along cylinderAxis.
            const componentAlongAxis = vecCapCenterToSphereCenter.dot(cylinderAxis);
            const projectionOnCapPlane = vecCapCenterToSphereCenter.clone().addScaledVector(cylinderAxis, -componentAlongAxis);
            
            // Scale this projected vector to cylinderRadius to get a point on the edge.
            projectionOnCapPlane.setLength(cylinderRadius);
            const pointOnCapEdge = new THREE.Vector3().copy(capCenter).add(projectionOnCapPlane);

            if (pointOnCapEdge.distanceToSquared(sphereCenter) < sphereRadius * sphereRadius) {
                return CollisionType.Partial;
            }
            return CollisionType.None;

        } else {
            // Sphere center is "laterally" inside or on the cylinder's radius.
            // (distSphereCenterToAxisSq <= cylinderRadius * cylinderRadius)

            // Check for total containment:
            // 1. Laterally: sphere fits within cylinder radius
            // 2. Height-wise: sphere fits within cylinder height
            if (Math.sqrt(distSphereCenterToAxisSq) + sphereRadius <= cylinderRadius && // Lateral containment
                projectionDistance >= sphereRadius &&                               // Bottom cap containment
                projectionDistance <= cylinderHeight - sphereRadius) {              // Top cap containment
                return CollisionType.Total;
            } else {
                // If not fully contained, but Conditions 1 & 2 (initial broad checks) passed,
                // and the sphere center's projection is within the cylinder's radius, it must be a partial collision.
                return CollisionType.Partial;
            }
        }
    }
}
