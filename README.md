# Mandelbrot & Julia Set Explorer

Explorador interativo em tempo real dos conjuntos de Mandelbrot e Julia, desenvolvido com C# e Raylib, utilizando shaders GLSL para renderização acelerada por GPU.

---

## O que foi implementado

- Renderização dos fractais via fragment shader GLSL — um cálculo por pixel, rodando na GPU em paralelo
- Dois modos de visualização: Mandelbrot Set e Julia Set
- Zoom e pan interativos com mouse
- Julia Set interativo — clique direito define o parâmetro `c` em tempo real
- Colorização HSV com gradiente suave
- Iterações adaptativas baseadas no nível de zoom
- FPS counter para monitoramento de performance

---

## Controles

| Ação | Funcionalidade |
|------|----------------|
| Botão esquerdo + arrastar | Pan (movimento da câmera) |
| Scroll do mouse | Zoom in/out |
| Shift (segurar) | Aumentar velocidade do zoom |
| TAB | Alternar entre Mandelbrot e Julia |
| Botão direito | Definir ponto `c` do Julia Set |
| ESC | Sair |

---

## Matemática

### Números complexos

Um número complexo `c = a + bi` é representado como `vec2(a, b)` no GLSL. A multiplicação de complexos:

```
(a + bi)(c + di) = (ac - bd) + (ad + bc)i
```

### Conjunto de Mandelbrot

Para cada pixel (número complexo `c`), itera:

```
z₀ = 0
zₙ₊₁ = zₙ² + c
```

Se `|z|` ultrapassa 2 → divergiu → cor baseada na velocidade de divergência.  
Se aguentar `maxIter` iterações → dentro do conjunto → preto.

### Julia Set

Mesma iteração, mas `c` é fixo e `z₀` começa no pixel:

```
z₀ = pixel
zₙ₊₁ = zₙ² + c
```

Clicar com o botão direito define o `c` — e a forma muda em tempo real.

---

## Detalhes técnicos

**Shader GLSL** — fragment shader recebe a posição do pixel, converte para coordenada complexa aplicando zoom e pan, itera a sequência e devolve a cor baseada na velocidade de divergência.

**Iterações adaptativas:**
```
maxIter = clamp(100 / zoom, 100, 1000)
```
Mais zoom → mais iterações → mais detalhe nas bordas.

**Colorização HSV:**
```glsl
float t = float(iter) / float(maxIter);
vec3 color = hsv2rgb(t * 0.7 + 0.6, 1.0, 1.0);
```
O `t` mapeia a velocidade de divergência para um matiz no espectro HSV. Interior do conjunto é sempre preto.

**Comunicação C# → GPU:**  
Os uniforms (`zoom`, `offset`, `juliaC`, `mode`, `maxIter`) são passados pelo Raylib a cada frame via `SetShaderValue`. O shader lê esses valores e os aplica no cálculo de cada pixel simultaneamente.

---

## Estrutura do projeto

```
MandelbrotJulia/
├── Program.cs           # Janela, input, uniforms
├── mandelbrot.frag      # Fragment shader GLSL
└── MandelbrotJulia.csproj
```

---

## Problemas conhecidos

- Zoom extremo pode causar artefatos — limite do `float32` (~7 dígitos de precisão). Zoom profundo real exige técnicas como perturbation theory com `double` ou arbitrary precision
- O shader requer OpenGL 3.3+

---

## Referências

- [The Mandelbrot Set — Wikipedia](https://en.wikipedia.org/wiki/Mandelbrot_set)
- [Julia Set — Wikipedia](https://en.wikipedia.org/wiki/Julia_set)
- [Raylib-cs](https://github.com/ChrisDill/Raylib-cs)
- [GLSL Core Language — Khronos](https://www.khronos.org/opengl/wiki/Core_Language_(GLSL))