#version 440 core

uniform vec2 resolution;
uniform float time;
uniform float zoom;
uniform vec2 viewpos;

in vec2 uv;
out vec4 outputColor;

// f(z) = z^2 + c

/*

	c = 1
	z = 0
	f(0) = 1
	f(1) = 2
	f(2) = 5
	f(5) = 26

	c = .5
	f(0) = .5
	f(.5) = .75
	f(.75) = 1.0625

	c = -1
	f(0) = -1
	f(-1) = 0
	f(0) = -1

	c = i
	f(0) = i
	f(i) = -1 + i
	f(-1 + i) = -i
	f(-i) = i

*/

const float maxiter = 800;

const float pi = 3.14159265;
const float tau = pi*2.;

vec3 randColor(float t) {
	return sin(vec3(t*2., t, t*3.));
}

void main() {

	vec2 c = (uv * zoom) - viewpos;
	c.x *= resolution.x / resolution.y;

	vec2 z = vec2(.0);
	int i = 0;
	while (i < maxiter) {
		z = vec2(z.x*z.x - z.y*z.y, 2*z.x*z.y) + c;

		if(length(z)>2.) break;	

		i++;
	}

	float f = i/maxiter;
	outputColor = vec4(randColor(f * tau), 1.);
}