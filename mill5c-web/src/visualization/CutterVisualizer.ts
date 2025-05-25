import * as THREE from 'three';
import { Cutter, CutterType } from '../core/Cutter';
// import { DEFAULT_ORIENTATION } from '../core/PathPoint'; // Not needed as per new version

export class CutterVisualizer {
    private scene: THREE.Scene;
    private cutterMeshGroup: THREE.Group;
    
    private sharedCutterMaterial = new THREE.MeshPhongMaterial({ color: 0xA0A0A0, specular: 0x888888, shininess: 60 });
    // private cylinderMaterial = new THREE.MeshPhongMaterial({ color: 0xA0A0A0, specular: 0x888888, shininess: 60 });
    // private sphereMaterial = new THREE.MeshPhongMaterial({ color: 0xA0A0A0, specular: 0x888888, shininess: 60 });

    // Store current parameters to detect changes
    private currentCutterParams: { radius: number, height: number, type: CutterType } | null = null;
    private cylinderGeom: THREE.CylinderGeometry | null = null;
    private sphereGeom: THREE.SphereGeometry | null = null;

    private cylinderMesh: THREE.Mesh | null = null;
    private sphereMesh: THREE.Mesh | null = null; 

    constructor(scene: THREE.Scene) {
        this.scene = scene;
        this.cutterMeshGroup = new THREE.Group();
        this.scene.add(this.cutterMeshGroup);
    }

    public update(cutter: Cutter): void {
        // Check if cutter parameters that define its shape have changed
        if (
            !this.currentCutterParams ||
            this.currentCutterParams.radius !== cutter.radius ||
            this.currentCutterParams.height !== cutter.height ||
            this.currentCutterParams.type !== cutter.type
        ) {
            this.createMeshes(cutter); // Recreate meshes if shape changes
            this.currentCutterParams = { radius: cutter.radius, height: cutter.height, type: cutter.type };
        }

        // Position the group (which represents the cutter's tip/base point)
        this.cutterMeshGroup.position.copy(cutter.position);

        if (this.cylinderMesh) {
            // Cylinder's origin is its center.
            // For a flat cutter, the cylinder's bottom face should be at the group's origin.
            // For a ball cutter, the sphere's center is at the group's origin (tip of cutter),
            // and the cylinder shaft should be "above" this.
            if (cutter.type === CutterType.Flat) {
                this.cylinderMesh.position.y = cutter.height / 2; 
                if (this.sphereMesh) this.sphereMesh.visible = false;
            } else if (cutter.type === CutterType.Ball) {
                this.cylinderMesh.position.y = cutter.height / 2; // Shaft "above" the sphere
                if (this.sphereMesh) {
                    this.sphereMesh.visible = true;
                    this.sphereMesh.position.y = 0; // Sphere center is at group origin
                }
            }
        }
        
        // Orientation:
        // Three.js CylinderGeometry is Y-up by default.
        const defaultCylinderUp = new THREE.Vector3(0, 1, 0); 
        const targetOrientation = cutter.orientation.clone().normalize();

        const quaternion = new THREE.Quaternion();
        quaternion.setFromUnitVectors(defaultCylinderUp, targetOrientation);
        this.cutterMeshGroup.quaternion.copy(quaternion);
    }
    
    private createMeshes(cutter: Cutter): void {
        this.clearMeshes(); 

        // Cylinder part (shaft)
        this.cylinderGeom = new THREE.CylinderGeometry(cutter.radius, cutter.radius, cutter.height, 32);
        this.cylinderMesh = new THREE.Mesh(this.cylinderGeom, this.sharedCutterMaterial); // Use shared material
        this.cutterMeshGroup.add(this.cylinderMesh);

        if (cutter.type === CutterType.Ball) {
            // Only create sphere geometry if it doesn't exist or radius changed
            if (!this.sphereGeom || this.currentCutterParams?.radius !== cutter.radius) {
                if (this.sphereGeom) this.sphereGeom.dispose();
                this.sphereGeom = new THREE.SphereGeometry(cutter.radius, 32, 16);
            }
            if (!this.sphereMesh) { // Create mesh if it doesn't exist
                this.sphereMesh = new THREE.Mesh(this.sphereGeom, this.sharedCutterMaterial); // Use shared material
                this.cutterMeshGroup.add(this.sphereMesh);
            }
            this.sphereMesh.visible = true;
        } else {
            if (this.sphereMesh) {
                this.sphereMesh.visible = false; 
            }
        }
    }
    
    public clear(): void { 
        this.clearMeshes();
        this.currentCutterParams = null; 
    }

    private clearMeshes(): void { 
        if (this.cylinderMesh) {
            this.cutterMeshGroup.remove(this.cylinderMesh);
            this.cylinderMesh.geometry.dispose(); 
            this.cylinderMesh = null;
        }
        if (this.sphereMesh) {
            this.cutterMeshGroup.remove(this.sphereMesh);
            // Sphere geometry is managed separately now due to potential reuse
            this.sphereMesh = null; 
        }
        // Dispose geometries if they are no longer needed by any mesh
        if (this.cylinderGeom) { this.cylinderGeom.dispose(); this.cylinderGeom = null; }
        // Only dispose sphereGeom if we are sure no mesh (even hidden) is using it.
        // If sphereMesh is null and type is not Ball, it's safe to dispose
        if (this.sphereGeom && !this.sphereMesh && this.currentCutterParams?.type !== CutterType.Ball) {
            this.sphereGeom.dispose(); this.sphereGeom = null;
        }
    }

    public disposeMaterials(): void {
        this.sharedCutterMaterial.dispose();
        // this.cylinderMaterial.dispose(); // No longer separate
        // this.sphereMaterial.dispose(); // No longer separate
    }
}
