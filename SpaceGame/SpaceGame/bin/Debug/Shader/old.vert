#version 120

uniform mat4 mod_mat;

attribute vec2 pos;
attribute vec3 vertex;

varying vec2 tex_coord;

void main()
{
	tex_coord = pos;
	gl_Position = mod_mat * vec4(vertex,  1);
}

