#version 330

out vec4 finalColor;
uniform vec2 resolution;
uniform float zoom;
uniform vec2 offset;
uniform int maxIter;

//Julia set
uniform vec2 juliaC;
uniform int mode;

vec3 hsv2rgb(float h, float s, float v) {
    vec3 c = vec3(h * 6.0, s, v);
    vec3 rgb = clamp(abs(mod(c.x + vec3(0.0, 4.0, 2.0), 6.0) - 3.0) - 1.0, 0.0, 1.0);
    
    return v * mix(vec3(1.0), rgb, s);
}

void main(){
    vec2 uv = gl_FragCoord.xy / resolution;
    vec2 z, c;

    //Definindo modo
    if(mode == 0) {
        z = vec2(0.0);
        c = (uv - vec2(0.5)) * vec2(3.5, 2.0) * zoom + offset;
    } else {
        z = (uv - vec2(0.5)) * vec2(3.5, 2.0) * zoom + offset;
        c = juliaC;
    }

    int iter = 0;
    
    for(int i = 0; i < maxIter; i++){
        z = vec2(z.x*z.x - z.y*z.y, 2.0*z.x*z.y) + c;
        if(dot(z,z) > 4.0) break;
        iter++;
    }
    
    //float t = float(iter) / float(maxIter);
    if (iter == maxIter) {
        finalColor = vec4(0.0, 0.0, 0.0, 1.0);
    } else {
        float t = float(iter) / float(maxIter);

        //Mudar esse valor altera a cor de partida
        vec3 color = hsv2rgb(t * 0.7 + 0.6, 1.0, 1.0);
        finalColor = vec4(color, 1.0);
    }
}