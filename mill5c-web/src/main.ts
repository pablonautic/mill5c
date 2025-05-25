import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js';
import { Octree } from './core/Octree';
import { Point3D } from './core/Geometry';
import { OctreeVisualizer } from './visualization/OctreeVisualizer';
import { NodeColor } from './core/NodeColor';
import { Cutter, CutterType, createDefaultCutter } from './core/Cutter'; // Added
import { CutterVisualizer } from './visualization/CutterVisualizer'; // Added

// Scene
const scene = new THREE.Scene();
scene.background = new THREE.Color(0xeeeeee); // Light gray background

// Camera
const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
camera.position.set(5, 5, 10); // Adjusted camera position for better view
camera.lookAt(0, 0, 0);

// Renderer
const renderer = new THREE.WebGLRenderer({ antialias: true });
renderer.setSize(window.innerWidth, window.innerHeight);
document.body.appendChild(renderer.domElement);

// Lights
const ambientLight = new THREE.AmbientLight(0xffffff, 0.6);
scene.add(ambientLight);
const directionalLight = new THREE.DirectionalLight(0xffffff, 0.8);
directionalLight.position.set(10, 20, 5);
scene.add(directionalLight);

// OrbitControls
const controls = new OrbitControls(camera, renderer.domElement);
controls.enableDamping = true;
controls.dampingFactor = 0.05;
controls.screenSpacePanning = false;
controls.minDistance = 1;
controls.maxDistance = 500;

// Octree and Visualizer
const octreeCenter = new THREE.Vector3(0, 0, 0);
const octreeSize = 10;
const octree = new Octree(octreeCenter, octreeSize, 0.5);

// --- Create a simple test octree structure ---
octree.rootNode.color = NodeColor.Partial;
octree.rootNode.split();
if (octree.rootNode.children) {
    if(octree.rootNode.children[0]) octree.rootNode.children[0].color = NodeColor.Solid;
    if(octree.rootNode.children[1]) {
        octree.rootNode.children[1].color = NodeColor.Partial;
        octree.rootNode.children[1].split();
        if (octree.rootNode.children[1].children) {
             if(octree.rootNode.children[1].children[0]) octree.rootNode.children[1].children[0].color = NodeColor.Solid;
             if(octree.rootNode.children[1].children[1]) octree.rootNode.children[1].children[1].color = NodeColor.Partial;
        }
    }
    if(octree.rootNode.children[3]) octree.rootNode.children[3].color = NodeColor.Empty;
}
// --- End of test octree structure ---

const octreeVisualizer = new OctreeVisualizer(scene);
octreeVisualizer.visualize(octree);

// Cutter and Visualizer (Added)
const sampleCutter = createDefaultCutter(CutterType.Ball, 0.5, 3); // type, radius, height
sampleCutter.position.set(octreeSize / 2 + 1, octreeSize / 2 + 1, 3); // Position it a bit away
sampleCutter.orientation.set(0, 0, 1).normalize(); // Default Z-up for now

const cutterVisualizer = new CutterVisualizer(scene);
// Initial update
cutterVisualizer.update(sampleCutter);


// Animation loop
let time = 0;
function animate() {
    requestAnimationFrame(animate);
    time += 0.01;

    // Example: Move cutter in a circle and wiggle orientation
    // sampleCutter.position.x = Math.sin(time * 0.5) * 5 + (octreeSize / 2 + 1);
    // sampleCutter.position.z = Math.cos(time * 0.5) * 5 + 3;
    // sampleCutter.orientation.x = Math.sin(time * 2) * 0.5;
    // sampleCutter.orientation.z = Math.cos(time * 2) * 0.5;
    // sampleCutter.orientation.normalize();
    // cutterVisualizer.update(sampleCutter); // Call update if cutter changes

    controls.update(); 
    renderer.render(scene, camera);
}

// Handle window resize
window.addEventListener('resize', () => {
    camera.aspect = window.innerWidth / window.innerHeight;
    camera.updateProjectionMatrix();
    renderer.setSize(window.innerWidth, window.innerHeight);
});

animate();

console.log('Three.js app with Octree and Cutter visualization initialized.');
