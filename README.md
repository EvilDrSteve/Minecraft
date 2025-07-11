
# Voxel Engine and Terrain Generation


This started as a personal project to create a custom voxel engine, I then hooked up the voxel engine to one of my old terrain generation projects, trying to recreate Minecraft-style terrain. 

These are all the raw code files that are used in the project, so this repository is a WIP.




## Resources used

 - [Sebestian Lague's Procedural Generation](https://youtu.be/wbpMiKiSKm8?si=r_iXaQFBVS6mdKKa)
 - [Henrik Kniberg's Minecraft terrain generation in a nutshell](https://youtu.be/CSa5O6knuwI?si=gYtPy-w3TcDHKZRY)
 - [Making Minecraft from scratch in 48 hours (NO GAME ENGINE)](https://youtu.be/4O0_-1NaWnY?si=O7kUr3GNHyxbSQWx)


## How the Voxel Engine works

### Triangles
In 3D graphics, shapes are made up of triangles. For example, a cube is composed of six sides, each represented by two triangles, totaling 12 triangles. Triangles consist of vertices, and in a cube, each triangle has three vertices, resulting in a total of 24 vertices for the entire cube.

### Rendering
After determining the placement and configuration of triangles to form a cube, the next step involves drawing these triangles on the screen to render the cube. Importantly, when multiple cubes are involved, they are still collectively represented as a single mesh. A mesh is a structure that connects the vertices, edges, and faces of each cube, allowing for unified and efficient rendering.

### Chunks
In Unity, there's a limit of 65535 maximum vertices on a single mesh. This means you can't really create a lot of cubes, approximately 5461 cubes. To overcome this limitation, we use the concept of chunks, dividing the cubes into different meshes. Each chunk represents a manageable portion, and I've chosen to divide areas into chunks of 16x100x16 for efficient handling.

## Optimizations

As previously mentioned, a single mesh can accommodate approximately 5000 cubes. To address this limitation, we've divided our cubes into 16x100x16 chunks. While this might seem to exceed the 5000-cube limit, we have an additional strategy to overcome mesh constraints: face culling. Let's explore how face culling plays a crucial role in optimizing and managing these voxel chunks.

### What is Face Culling?
Consider a cube resting on the ground – no matter our perspective, we cannot see its bottom face. Likewise, in our scenario, specific faces are perpetually out of view. So, why render what can't be seen? Face culling is a technique where we strategically choose to render only the faces that are visible, optimizing rendering by excluding those that remain unseen.

### After Face Culling
In simple words, no underground blocks are rendered – they exist but remain unseen. This results in significant space savings, as compared to the example with all 16 x 100 blocks being rendered. With face culling, we achieve a more efficient use of resources, optimizing rendering and enhancing overall performance.

## Procedural Generation
With a functional voxel engine that allows us to render cubes on demand, the next step is to integrate it with the Perlin noise terrain generator. Let's explore the fascinating landscapes we can create by combining these elements. In the upcoming five steps, we'll delve into the process of how the terrain is pieced together.

### 1. Terrain Generator
Directly integrating the algorithm from our terrain generator project into our voxel engine yields terrain resembling hills and rivers. However, this landscape, while initially intriguing, lacks diversity and becomes repetitive at larger scales.

### 2. Redistribution Modifier
Perlin noise provides balanced inputs, resulting in half of our terrain being hills and half valleys/rivers. However, this equilibrium means over half of the world appears underwater. To address this, we implement a redistribution technique, exaggerating noise values by taking their exponents. This adjustment pushes most valleys further down, providing more space above water to work with and yielding higher peaks in the landscape.

### 3. Water Level
As a consequence of pushing the ground further down, the world predominantly appears as water. To rectify this, we lower the water level to align with the landscape, reducing it from, for instance, 50 blocks to 10 blocks.

### 4. Octaves
Our terrain has improved, but it lacks the desired level of complexity; it's too smooth. To enhance it, we introduce the concept of octaves. Initially, we establish the basic shape of the terrain. Next, we reduce the scale of our noise, generating smaller variations, and layer it onto the existing terrain. This addition introduces more bumps and details. We repeat this process multiple times, gradually decreasing the scale with each iteration. This layering of different scales creates a rich and intricate landscape, adding the desired level of detail and realism to our terrain.

### 5. Terraforming
The terrain now boasts a more natural shape, but to enhance its appeal, we introduce spots of stone and podzol. Employing the same noise algorithm with slightly different settings, we layer these variations onto our existing terrain. This addition creates pockets of diverse elements, adding interest and complexity to the landscape.
