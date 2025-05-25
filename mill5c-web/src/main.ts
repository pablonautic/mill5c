import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls.js';
// Core components
import { Octree } from './core/Octree'; // Used by SimulationController internally
import { Point3D } from './core/Geometry'; // Used for initialOctreeCenter
import { PathPoint, createDefaultPathPoint } from './core/PathPoint'; // Used for path data
import { parsePathFile } from './core/PathParser';
import { CutterType } from './core/Cutter';
// Simulation and Visualization
import { SimulationController } from './simulation/SimulationController';

// --- Basic Three.js Setup ---
// Scene
const scene = new THREE.Scene();
scene.background = new THREE.Color(0xeeeeee); // Light gray background

// Axes Helper
const axesHelper = new THREE.AxesHelper(5); // Length of 5 units for axes
scene.add(axesHelper);

// Camera
const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
camera.position.set(5, 10, 15); // Adjusted camera position for better overall view
camera.lookAt(0, 0, 0);

// Renderer
const renderer = new THREE.WebGLRenderer({ antialias: true });
renderer.setSize(window.innerWidth, window.innerHeight);
document.body.appendChild(renderer.domElement);

// Lights
const ambientLight = new THREE.AmbientLight(0xffffff, 0.7); // Slightly brighter ambient
scene.add(ambientLight);
const directionalLight = new THREE.DirectionalLight(0xffffff, 0.9); // Slightly brighter directional
directionalLight.position.set(10, 20, 15);
directionalLight.castShadow = true; // Optional: if you want shadows
scene.add(directionalLight);

// OrbitControls
const controls = new OrbitControls(camera, renderer.domElement);
controls.enableDamping = true;
controls.dampingFactor = 0.05;
controls.screenSpacePanning = false;
controls.minDistance = 1;
controls.maxDistance = 500;
controls.target.set(0, 2, 0); // Adjust target to look slightly above origin if octree is at y=0
controls.update();


// --- Simulation Setup ---
const initialOctreeCenter = new THREE.Vector3(0, 0, 0);
const initialOctreeSize = 10;
const initialMinNodeHalfSize = 0.25; // Adjusted for potentially coarser octree

const simController = new SimulationController(
    scene,
    initialOctreeCenter,
    initialOctreeSize,
    initialMinNodeHalfSize
);

// --- Test Path Data ---
const testPathString = `
N1 G01 X0 Y0 Z10 I0 J0 K1 F100
N2 G01 Z5
N3 G01 X5 Y0 Z5
N4 G01 X5 Y5 Z5
N5 G01 X0 Y5 Z5
N6 G01 X0 Y0 Z5
N7 G01 Z10
`; 
// Note: F (feed rate) is ignored by our current parser. I,J,K are for orientation.

const parsedPathPoints = parsePathFile(testPathString);

if (parsedPathPoints.length > 0) {
    simController.loadPath(parsedPathPoints, 0.5, 4, CutterType.Ball); // radius, height, type
    console.log("Test path loaded into SimulationController.");
    // To start simulation automatically (optional):
    // simController.start(); 
} else {
    console.error("Failed to parse test path data. Visualizing empty octree and default cutter.");
    // Fallback: visualize empty octree and default cutter if path parsing fails
    simController.loadPath([], 0.5, 4, CutterType.Ball); 
}

// --- UI Buttons ---
const uiContainer = document.createElement('div');
uiContainer.style.position = 'absolute';
uiContainer.style.top = '10px';
uiContainer.style.left = '10px';
uiContainer.style.zIndex = '100';
document.body.appendChild(uiContainer);

function addButton(text: string, onClick: () => void): HTMLButtonElement {
    const button = document.createElement('button');
    button.textContent = text;
    button.onclick = onClick;
    button.style.margin = '2px';
    uiContainer.appendChild(button);
    return button;
}

addButton('Start', () => simController.start());
addButton('Pause', () => simController.pause());
addButton('Resume', () => simController.resume());
addButton('Step', () => simController.stepOnce());
addButton('Reset', () => {
    // Re-load the path or load a default empty one
    if (parsedPathPoints.length > 0) {
        simController.loadPath(parsedPathPoints, 0.5, 4, CutterType.Ball);
    } else {
        simController.loadPath([], 0.5, 4, CutterType.Ball);
    }
});


// --- Main Animation Loop (for rendering and controls) ---
function animate() {
    requestAnimationFrame(animate);
    controls.update(); // For damping or auto-rotate
    // The SimulationController manages its own gameLoop via requestAnimationFrame
    // for simulation logic, so we don't call it directly here.
    renderer.render(scene, camera);
}
animate();

// --- Cleanup ---
window.addEventListener('beforeunload', () => {
    simController.dispose();
});

console.log('Three.js app with SimulationController initialized.');
