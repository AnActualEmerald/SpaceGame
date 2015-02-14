#version 330

uniform sampler2D texture;

in vec2 tex_coord;

out vec4 color;

void main()
{
	//color = vec4(tex_coord, 1, 1);
	color = texture2D(texture, tex_coord);
}