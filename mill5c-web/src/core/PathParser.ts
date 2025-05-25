import * as THREE from 'three';
import { Point3D, Vector3D } from './Geometry';
import { PathPoint, createDefaultPathPoint, DEFAULT_ORIENTATION } from './PathPoint';

// Regular expression to capture a coordinate and its value (e.g., X123.456)
// It captures the axis letter and the floating point number.
// This one is not used in parseSingleCoordinate, but could be useful for other parsing.
// const coordRegex = /([XYZIJK])(-?\d*\.?\d+)/g;

function parseSingleCoordinate(line: string, axis: 'X' | 'Y' | 'Z' | 'I' | 'J' | 'K'): number | null {
    // More robust regex to find specific axis values
    const axisRegex = new RegExp(`${axis}(-?\\d*\\.?\\d+)`);
    const match = line.match(axisRegex);
    if (match && match[1]) {
        return parseFloat(match[1]);
    }
    return null;
}

export function parsePathLine(line: string, previousPoint: PathPoint): PathPoint {
    const newPosition = previousPoint.position.clone();
    const newOrientation = previousPoint.orientation.clone();

    const x = parseSingleCoordinate(line, 'X');
    if (x !== null) newPosition.x = x;

    const y = parseSingleCoordinate(line, 'Y');
    if (y !== null) newPosition.y = y;

    const z = parseSingleCoordinate(line, 'Z');
    if (z !== null) newPosition.z = z;

    const i = parseSingleCoordinate(line, 'I');
    const j = parseSingleCoordinate(line, 'J');
    const k = parseSingleCoordinate(line, 'K');

    let orientationChanged = false;
    if (i !== null) { newOrientation.x = i; orientationChanged = true; }
    if (j !== null) { newOrientation.y = j; orientationChanged = true; }
    if (k !== null) { newOrientation.z = k; orientationChanged = true; }

    if (orientationChanged) {
        newOrientation.normalize();
    }

    return { position: newPosition, orientation: newOrientation };
}

export function parsePathFile(fileContent: string): PathPoint[] {
    const pathPoints: PathPoint[] = [];
    const lines = fileContent.split('\n').map(line => line.trim()).filter(line => line.length > 0);

    let previousPoint = createDefaultPathPoint(); // Initialize with a default starting point

    for (const line of lines) {
        // Skip lines that don't look like G-code commands with N-numbers,
        // or adapt if other line formats are expected.
        // For now, assuming relevant lines start with N and contain G01 based on C#
        if (!line.startsWith('N') || !line.includes('G01')) {
            // Potentially log skipped lines or handle them if necessary
            // console.warn(`Skipping line (doesn't match expected format N...G01...): ${line}`);
            continue;
        }
        
        const currentPoint = parsePathLine(line, previousPoint);
        pathPoints.push(currentPoint);
        previousPoint = currentPoint;
    }

    if (pathPoints.length === 0 && lines.length > 0) {
         console.warn("Path file parsed but resulted in zero path points. Check file format and parsing logic.");
    }
    
    return pathPoints;
}
