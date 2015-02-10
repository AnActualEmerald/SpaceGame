#version 330

uniform sampler2D texture;

in vec2 tex_coord;

out vec4 color;

void main()
{
	color = vec4(0, 1, 0, 1);//vec4(texture2D(texture, tex_coord).xyz, 0.5);
}