#version 330

uniform mat4 mod_mat;

layout(location = 0)in vec2 pos;
layout(location = 1)in vec3 vertex;

out vec2 tex_coord;

void main()
{
	tex_coord = pos;
	gl_Position = mod_mat * vec4(vertex,  1);
}

