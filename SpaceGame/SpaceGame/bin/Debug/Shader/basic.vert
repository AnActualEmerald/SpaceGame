#version 330

uniform mat4 mod_mat;
uniform vec2 offset;

layout(location = 0)in vec2 pos;
layout(location = 1)in vec2 vertex;

out vec2 tex_coord;

void main()
{
	tex_coord = vertex;
	gl_Position = vec4(vertex, 0.5, 1);
}