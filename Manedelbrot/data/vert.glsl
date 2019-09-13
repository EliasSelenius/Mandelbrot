#version 440 core


in vec2 v_pos;
out vec2 uv;

void main() {
	uv = v_pos;
	gl_Position = vec4(v_pos, 0., 1.);
}