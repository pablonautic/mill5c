using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// using InstancedModelSample; // Removed dependency
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mill5C.Core.DataStructures;
using Mill5C.Core.Materials;

namespace Mill5C.View.Window.Renderers.MonoGame
{
    public abstract class OctreeRendererBase : MonoGameRendererBase 
    {
        public bool Cubes { get; private set; }

        // Replace InstancedModel with VertexBuffer and IndexBuffer for a single cube/sphere
        protected VertexBuffer_CubeInstance cubeVertexBuffer;
        protected IndexBuffer_CubeInstance cubeIndexBuffer;
        protected int cubeIndexCount; // Store the number of indices for the cube/sphere

        protected Matrix[] matrices; // Holds world transforms for each instance

        public OctreeRendererBase(bool cubes)
        {
            Cubes = cubes;
        }

        public override void Initialize(Mill5C.Core.Algorithm.Engine engine, object scene)
        {
            base.Initialize(engine, scene);
            // Vertex/Index buffer creation moved to LoadContent as it requires GraphicsDevice
        }

        public override void LoadContent()
        {
            base.LoadContent(); // This initializes GameViewModel.GraphicsDevice and GameViewModel.Content

            if (Cubes)
            {
                CreateCubeBuffers();
            }
            else
            {
                CreateSphereBuffers(); // Placeholder - sphere generation is more complex
            }
        }

        private void CreateCubeBuffers()
        {
            var vertices = new VertexPositionNormalTexture[24]; // Cube: 6 faces * 4 vertices
            var indices = new short[36]; // 6 faces * 2 triangles * 3 indices

            // Define a unit cube (size 1x1x1 centered at origin)
            // Front face
            vertices[0] = new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, 0.5f), Vector3.Forward, new Vector2(0, 1));
            vertices[1] = new VertexPositionNormalTexture(new Vector3(0.5f, -0.5f, 0.5f), Vector3.Forward, new Vector2(1, 1));
            vertices[2] = new VertexPositionNormalTexture(new Vector3(0.5f, 0.5f, 0.5f), Vector3.Forward, new Vector2(1, 0));
            vertices[3] = new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, 0.5f), Vector3.Forward, new Vector2(0, 0));
            // Back face
            vertices[4] = new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, -0.5f), Vector3.Backward, new Vector2(1, 1));
            vertices[5] = new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, -0.5f), Vector3.Backward, new Vector2(1, 0));
            vertices[6] = new VertexPositionNormalTexture(new Vector3(0.5f, 0.5f, -0.5f), Vector3.Backward, new Vector2(0, 0));
            vertices[7] = new VertexPositionNormalTexture(new Vector3(0.5f, -0.5f, -0.5f), Vector3.Backward, new Vector2(0, 1));
            // Top face
            vertices[8] = new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, 0.5f), Vector3.Up, new Vector2(0, 1));
            vertices[9] = new VertexPositionNormalTexture(new Vector3(0.5f, 0.5f, 0.5f), Vector3.Up, new Vector2(1, 1));
            vertices[10] = new VertexPositionNormalTexture(new Vector3(0.5f, 0.5f, -0.5f), Vector3.Up, new Vector2(1, 0));
            vertices[11] = new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, -0.5f), Vector3.Up, new Vector2(0, 0));
            // Bottom face
            vertices[12] = new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, -0.5f), Vector3.Down, new Vector2(0, 1));
            vertices[13] = new VertexPositionNormalTexture(new Vector3(0.5f, -0.5f, -0.5f), Vector3.Down, new Vector2(1, 1));
            vertices[14] = new VertexPositionNormalTexture(new Vector3(0.5f, -0.5f, 0.5f), Vector3.Down, new Vector2(1, 0));
            vertices[15] = new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, 0.5f), Vector3.Down, new Vector2(0, 0));
            // Left face
            vertices[16] = new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, -0.5f), Vector3.Left, new Vector2(0, 1));
            vertices[17] = new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, -0.5f), Vector3.Left, new Vector2(1, 1));
            vertices[18] = new VertexPositionNormalTexture(new Vector3(-0.5f, 0.5f, 0.5f), Vector3.Left, new Vector2(1, 0));
            vertices[19] = new VertexPositionNormalTexture(new Vector3(-0.5f, -0.5f, 0.5f), Vector3.Left, new Vector2(0, 0));
            // Right face
            vertices[20] = new VertexPositionNormalTexture(new Vector3(0.5f, -0.5f, 0.5f), Vector3.Right, new Vector2(0, 1));
            vertices[21] = new VertexPositionNormalTexture(new Vector3(0.5f, 0.5f, 0.5f), Vector3.Right, new Vector2(1, 1));
            vertices[22] = new VertexPositionNormalTexture(new Vector3(0.5f, 0.5f, -0.5f), Vector3.Right, new Vector2(1, 0));
            vertices[23] = new VertexPositionNormalTexture(new Vector3(0.5f, -0.5f, -0.5f), Vector3.Right, new Vector2(0, 0));

            // Indices for 12 triangles (6 faces)
            short[] currentIndices = {
                0, 1, 2, 0, 2, 3, // Front
                4, 5, 6, 4, 6, 7, // Back
                8, 9, 10, 8, 10, 11, // Top
                12, 13, 14, 12, 14, 15, // Bottom
                16, 17, 18, 16, 18, 19, // Left
                20, 21, 22, 20, 22, 23  // Right
            };
            Array.Copy(currentIndices, indices, currentIndices.Length);
            cubeIndexCount = indices.Length;

            cubeVertexBuffer = new VertexBuffer_CubeInstance(GameViewModel.GraphicsDevice, typeof(VertexPositionNormalTexture), vertices.Length, BufferUsage.WriteOnly);
            cubeVertexBuffer.SetData(vertices);

            cubeIndexBuffer = new IndexBuffer_CubeInstance(GameViewModel.GraphicsDevice, typeof(short), indices.Length, BufferUsage.WriteOnly);
            cubeIndexBuffer.SetData(indices);
        }
        
        private void CreateSphereBuffers()
        {
            // Placeholder for sphere generation - this is complex.
            // For now, we'll just make a very simple approximation or use the cube as a fallback.
            // To keep it simple for this step, let's reuse cube logic for sphere as a placeholder.
            System.Diagnostics.Debug.WriteLine("Sphere rendering requested, using cube as placeholder.");
            CreateCubeBuffers(); 
        }


        public override void Draw()
        {
            if (matrices == null || !Visible || cubeVertexBuffer == null || cubeIndexBuffer == null)
                return;

            base.Draw(); // Sets up effect matrices in MonoGameRendererBase (textureEffect, colorEffect)
            
            GameViewModel.GraphicsDevice.SetVertexBuffer(cubeVertexBuffer);
            GameViewModel.GraphicsDevice.Indices = cubeIndexBuffer;

            // Use colorEffect from base class (MonoGameRendererBase), which should have lighting enabled.
            // Or use textureEffect if texturing is desired (requires texture coordinates in vertex buffer).
            // The procedural cube has texture coords, so textureEffect could be used if a texture is set.
            // For simplicity, let's use colorEffect and set a default color, or enable vertex coloring.
            
            colorEffect.VertexColorEnabled = false; // If we want to color all instances the same via DiffuseColor
            colorEffect.LightingEnabled = true; // Ensure lighting is on
            colorEffect.DiffuseColor = Color.Gray.ToVector3(); // Default color for instances

            if (textureEffect.Texture == null) { // if no texture is set on textureEffect, use colorEffect
                 // Ensure textureEffect is not used if its texture is null
            } else {
                // Logic to choose textureEffect if a texture is set and desired.
                // For now, always use colorEffect.
            }


            foreach (Matrix instanceMatrix in matrices)
            {
                colorEffect.World = instanceMatrix; // Apply the specific instance's world transform
                // View and Projection are already set on the effect by base.Draw()
                
                foreach (EffectPass pass in colorEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    GameViewModel.GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, cubeIndexCount / 3);
                }
            }

            // Use GameViewModel.SpriteBatch
            if (GameViewModel.SpriteBatch != null && font != null) 
            {
                GameViewModel.SpriteBatch.Begin();
                GameViewModel.SpriteBatch.DrawString(font, String.Format("Instances: {0}", matrices.Length),
                    new Vector2(15, 15), Color.DimGray);
                GameViewModel.SpriteBatch.End();
            }
        }

        protected void Transverse()
        {
            if (Engine?.Material == null) // Null check for Engine and Material
            {
                matrices = Array.Empty<Matrix>(); // Ensure matrices is not null
                return;
            }
            var mat = (OctreeMaterial)Engine.Material;

            List<Matrix> nodeMatrices = new List<Matrix>();
            FillList(mat.Tree.Root, nodeMatrices);
            matrices = nodeMatrices.ToArray();
        }

        protected void FillList(Node node, List<Matrix> nodeMatrices)
        {
            if (node == null || node.Color == NodeColor.White)
                return;

            if (node.Children == null)
            {
                // Scale factor for sphere was 1.0f / 4.8f * node.L. A unit sphere (radius 0.5) needs scale L.
                // A unit cube (side 1) needs scale L.
                // The old code used "2 * node.L" for cubes, implying the base cube model was 0.5x0.5x0.5 or similar.
                // Our procedural cube is 1x1x1. So scale should be node.L for cube.
                // For sphere, if its radius is 0.5, scale should be node.L for diameter L, or 2*node.L for radius L.
                // Let's assume node.L is the desired side length/diameter.
                float scale = Cubes ? node.L : node.L; // Assuming unit cube/sphere used for node.L size
                if (!Cubes) scale *= 0.5f; // If unit sphere is radius 0.5, and node.L is diameter. Or adjust sphere generation.
                                         // The old sphere scale: 1.0f / 4.8f * node.L suggests a specific model size.
                                         // For a unit sphere (radius 0.5), to get diameter L, scale by L.
                                         // The old "sphere" model was likely not unit size.
                                         // Let's use node.L as the scale for our unit cube/sphere.

                nodeMatrices.Add(Matrix.CreateScale(scale) * // Apply scale first
                    Matrix.CreateTranslation(node.Center.X, node.Center.Y, node.Center.Z));
            }
            else
            {
                foreach (var child in node.Children)
                {
                    FillList(child, nodeMatrices);
                }
            }

        }
    }
}

// Helper type aliases for clarity, matching XNA's naming convention for these specific buffers
using VertexBuffer_CubeInstance = Microsoft.Xna.Framework.Graphics.VertexBuffer;
using IndexBuffer_CubeInstance = Microsoft.Xna.Framework.Graphics.IndexBuffer;
