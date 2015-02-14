#version 330

uniform sampler2D texture;

in vec2 tex_coord;

out vec4 color;

void main()
{
	color = texture2D(texture, tex_coord);
}