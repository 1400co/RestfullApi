---
description: Metodología de gestión de conocimiento, trazabilidad y artefactos para RestfullApi
alwaysApply: true
---

# RestfullApi — Reglas de Proyecto IA

Documento maestro para desarrollo asistido por IA en Cursor. Define la estructura de artefactos, convenciones y trazabilidad obligatoria. Complementa las reglas técnicas en `cSharp-rules.md`.

**Fuente de verdad del conocimiento:** carpeta `knowledge-base/`

---

## Objetivo

Toda información funcional, técnica y de negocio debe ser:
- Trazable
- Versionable
- Comprensible para humanos e IA
- Reutilizable durante todo el ciclo de vida del proyecto

---

## Principios obligatorios

### P1. Una sola fuente de verdad

- Las **reglas de negocio**, **entidades** y **glosario** en `knowledge-base/02-domain/` son la autoridad
- **Nunca inventar reglas de negocio** — si no existe documentación, crear el artefacto primero
- Toda funcionalidad debe estar respaldada por: Regla de negocio + Entidad + Actor

### P2. Trazabilidad completa

Cadena obligatoria:

```
Entrevista → Hallazgo → Regla de Negocio → Entidad → Épica → Feature → Historia → Criterio → Implementación → Prueba
```

### P3. Markdown como formato oficial

| Permitido | No recomendado (solo entregables externos) |
|-----------|---------------------------------------------|
| `.md`, `.mmd`, `.drawio`, `.json`, `.yaml` | `.docx`, `.pdf`, `.xlsx` |

### P4. Identificadores correlativos

Usar prefijos globales por tipo:

| Prefijo | Artefacto |
|---------|-----------|
| `H-XXX` | Hallazgo |
| `RN-XXX` | Regla de negocio |
| `ACT-XXX` | Actor |
| `ENT-XXX` | Entidad |
| `EVN-XXX` | Evento de negocio |
| `CU-XXX` | Caso de uso |
| `HU-XXX` | Historia de usuario |
| `CA-XXX` | Criterio de aceptación |
| `EP-XXX` | Épica |
| `FEAT-XXX` | Feature |
| `ADR-XXX` | Architecture Decision Record |
| `DEC-XXX` | Decisión funcional/negocio |
| `PA-XXX` | Pregunta abierta (pendiente de validación) |
| `PS-XXX` | Pregunta sugerida para próxima reunión |

---

## Estructura `knowledge-base/`

```
knowledge-base/
├── 01-discovery/        # Info del cliente — pendiente de validación
├── 02-domain/           # Negocio — CONSULTAR ANTES DE CODIFICAR
├── 03-requirements/     # Casos de uso, historias, criterios
├── 04-backlog/          # Épicas, features, roadmap
├── 05-architecture/     # Modelo, arquitectura, ADRs
├── 06-decisions/        # Decisiones funcionales
└── 07-ai-context/       # Contexto y prompts para agentes
```

---

## Flujo de trabajo del agente

### Antes de cualquier tarea

1. Leer `knowledge-base/02-domain/reglas-negocio.md`
2. Leer `knowledge-base/02-domain/entidades.md`
3. Leer `knowledge-base/02-domain/glosario.md`
4. Consultar `knowledge-base/07-ai-context/contexto-producto.md` si existe

### Antes de crear una historia de usuario

Verificar que existan:
- [ ] Actor (`ACT-XXX` en `actores.md`)
- [ ] Entidad (`ENT-XXX` en `entidades.md`)
- [ ] Regla de negocio (`RN-XXX` en `reglas-negocio.md`)

### Antes de generar código

Verificar que existan:
- [ ] Historia de usuario (`HU-XXX` en `03-requirements/historias/`)
- [ ] Regla de negocio asociada
- [ ] Entidad asociada
- [ ] Criterios de aceptación en formato Gherkin (`CA-XXX`)

### Si falta documentación

1. **No implementar** la funcionalidad directamente
2. Crear o actualizar los artefactos faltantes en `knowledge-base/` siguiendo los formatos de este documento
3. Mantener trazabilidad con referencias cruzadas (`Reglas Relacionadas`, `Entidades Relacionadas`, `Origen`)

---

## 01-Discovery — Información del cliente

**Estado:** información pendiente de validación — NO es verdad absoluta.

```
01-discovery/
├── entrevistas/
├── documentos/
├── procesos/
├── preguntas-abiertas.md
└── preguntas-proxima-reunion.md
```

### Regla crítica

Las entrevistas **NO generan historias de usuario directamente**. Primero convertir en:
- Hallazgos (`H-XXX`)
- Reglas de negocio (`RN-XXX`)
- Entidades (`ENT-XXX`)
- Actores (`ACT-XXX`)

### Formato de entrevista

```markdown
# Entrevista 001
Fecha:
Participantes:

## Hallazgos

### H-001
Descripción

### H-002
Descripción

## Preguntas Abiertas
- Pregunta 1
```

### Preguntas abiertas (`preguntas-abiertas.md`)

Registro acumulativo de dudas detectadas durante el análisis que **requieren validación del cliente**. Se actualiza al procesar cada entrevista.

```markdown
# Preguntas Abiertas

| ID | Pregunta | Área | Origen | Estado | Fecha |
|----|----------|------|--------|--------|-------|
| PA-001 | ¿Texto de la pregunta? | Dominio / Proceso / Técnico | Entrevista-001 / H-003 | Pendiente | YYYY-MM-DD |
```

- **Estado:** `Pendiente` | `Respondida` | `Descartada`
- Vincular siempre al origen (`Entrevista-XXX`, `H-XXX`)
- Si una pregunta se responde en una entrevista posterior, marcar como `Respondida` y referenciar la entrevista

### Preguntas sugeridas para próxima reunión (`preguntas-proxima-reunion.md`)

**Obligatorio generar o actualizar** después de cada análisis de entrevista. Contiene preguntas proactivas para la **siguiente reunión con el cliente**, derivadas de:
- Lagunas de información detectadas en la entrevista
- Hallazgos ambiguos o incompletos (`H-XXX`)
- Contradicciones entre lo dicho y artefactos existentes en `02-domain/`
- Temas no cubiertos pero necesarios para avanzar (actores, reglas, flujos, integraciones)

```markdown
# Preguntas Sugeridas — Próxima Reunión

Última actualización: YYYY-MM-DD
Basado en: Entrevista-001

## Contexto
Breve resumen de por qué estas preguntas son necesarias (1–3 oraciones).

## Preguntas por prioridad

### Alta prioridad
| ID | Pregunta | Motivo | Relacionado con |
|----|----------|--------|-----------------|
| PS-001 | ¿Pregunta concreta y accionable? | Por qué es crítica para avanzar | H-002, ENT-001 |

### Media prioridad
| ID | Pregunta | Motivo | Relacionado con |
|----|----------|--------|-----------------|
| PS-002 | ¿Pregunta? | Contexto | RN-001 |

### Baja prioridad / exploratoria
| ID | Pregunta | Motivo | Relacionado con |
|----|----------|--------|-----------------|
| PS-003 | ¿Pregunta? | Contexto | — |

## Temas a profundizar
- Tema 1: descripción breve de lo que falta aclarar
- Tema 2: descripción breve

## Participantes sugeridos
- Rol o persona que debería responder cada bloque de preguntas (si se conoce)
```

**Reglas para preguntas sugeridas (`PS-XXX`):**
- Formular preguntas **concretas y accionables**, no genéricas
- Agrupar por prioridad (`Alta` → bloqueantes para requerimientos; `Media` → importantes; `Baja` → exploratorias)
- Vincular cada pregunta al hallazgo, entidad o regla que la motiva
- No duplicar: si ya existe en `preguntas-abiertas.md` como `PA-XXX`, referenciar el ID en lugar de repetir
- Tras la siguiente entrevista: mover preguntas respondidas a `preguntas-abiertas.md` (estado `Respondida`) y regenerar `preguntas-proxima-reunion.md`

---

## 02-Domain — El negocio (prioridad máxima)

**Consultar siempre antes de:** código, historias, casos de uso, APIs, pruebas.

```
02-domain/
├── actores.md
├── entidades.md
├── estados.md
├── eventos-negocio.md
├── reglas-negocio.md
└── glosario.md
```

### Actor (`ACT-XXX`)

```markdown
# ACT-001 NombreActor
## Responsabilidades
- Acción 1

## Permisos
- Permiso 1
```

### Entidad (`ENT-XXX`)

```markdown
# ENT-001 NombreEntidad
## Descripción
Qué representa en el negocio.

## Atributos
- Id
- Nombre

## Relaciones
- OtraEntidad
```

### Estados

Máquina de estados por entidad en `estados.md`:

```markdown
# NombreEntidad
- Estado1
- Estado2
- Estado3
```

### Evento de negocio (`EVN-XXX`)

```markdown
# EVN-001 NombreEvento
Descripción: Qué sucedió y quién lo disparó.
```

### Regla de negocio (`RN-XXX`)

```markdown
# RN-001
Título: Título breve
Descripción: Restricción obligatoria del negocio.
Origen: Entrevista-001 / H-001
Impacto: Workflow | Validación | Seguridad | etc.
```

### Glosario

```markdown
# Término
Definición: Significado en el contexto del proyecto.
```

---

## 03-Requirements — Funcionalidades

```
03-requirements/
├── casos-uso/
├── historias/
└── criterios/
```

### Caso de uso (`CU-XXX`)

```markdown
# CU-001 NombreCaso
Actor: ACT-001

Flujo Principal:
1. Paso 1
2. Paso 2

Reglas Relacionadas:
- RN-001
```

### Historia de usuario (`HU-XXX`)

```markdown
# HU-001 NombreHistoria
Como [Actor]
Quiero [acción]
Para [beneficio]

## Reglas Relacionadas
- RN-001

## Entidades Relacionadas
- ENT-001

## Épica / Feature
- EP-001 / FEAT-001
```

### Criterio de aceptación (`CA-XXX`) — siempre Gherkin

```markdown
# CA-001
Dado [contexto]
Cuando [acción]
Entonces [resultado esperado]
```

---

## 04-Backlog — Gestión del trabajo

```
04-backlog/
├── epicas.md
├── features.md
└── roadmap.md
```

### Relación obligatoria

```
Épica (EP-XXX) → Feature (FEAT-XXX) → Historia (HU-XXX) → Criterio (CA-XXX)
```

### Épica (`EP-XXX`)

```markdown
# EP-001 NombreÉpica
Descripción: Capacidad de negocio de alto nivel.
```

### Feature (`FEAT-XXX`)

```markdown
# FEAT-001 NombreFeature
Épica: EP-001
Descripción: Subdivisión implementable de la épica.
Historias: HU-001, HU-002
```

---

## 05-Architecture — Solución técnica

```
05-architecture/
├── modelo-dominio.md
├── arquitectura.md
├── integraciones.md
└── adr/
```

### ADR (`ADR-XXX`)

```markdown
# ADR-001
Estado: Aceptada | Propuesta | Deprecada
Contexto: Problema a resolver.
Decisión: Qué se decidió.
Consecuencias: Impacto técnico y de negocio.
```

> Las decisiones técnicas de implementación (Clean Architecture, JWT, etc.) están en `cSharp-rules.md` y `README.md`.

---

## 06-Decisions — Decisiones de negocio

```markdown
# DEC-001
Título: Título de la decisión
Fecha: YYYY-MM-DD
Decisión: Qué se acordó.
Justificación: Por qué.
```

---

## 07-AI-Context — Contexto para agentes

```
07-ai-context/
├── contexto-producto.md
├── prompt-pm.md
├── prompt-analista.md
├── prompt-dev.md
└── uso-subagente-analista.md   # Guía del subagente Cursor /analista
```

Consultar estos archivos según el rol de la tarea (análisis, desarrollo, gestión).

**Subagente Cursor:** `.cursor/agents/analista.md` — ver `knowledge-base/07-ai-context/uso-subagente-analista.md`

---

## Reglas para el agente de Cursor

### Siempre

1. Leer `02-domain/` antes de proponer o escribir código
2. Usar identificadores correlativos (`RN-001`, `HU-001`, etc.)
3. Referenciar artefactos relacionados en cada documento nuevo
4. Escribir criterios de aceptación en **formato Gherkin**
5. Al implementar código, seguir además `cSharp-rules.md`
6. Tras analizar una entrevista, actualizar `preguntas-abiertas.md` y `preguntas-proxima-reunion.md`

### Nunca

1. Crear funcionalidades sin respaldo documental en `knowledge-base/`
2. Inventar reglas de negocio no validadas
3. Saltar la cadena de trazabilidad
4. Convertir entrevistas directamente en historias de usuario
5. Usar formatos `.docx`, `.pdf`, `.xlsx` para artefactos internos

### Al procesar una entrevista

1. Colocar acta en `01-discovery/entrevistas/`
2. Extraer hallazgos (`H-XXX`)
3. **Actualizar `01-discovery/preguntas-abiertas.md`** con dudas detectadas (`PA-XXX`)
4. **Generar o actualizar `01-discovery/preguntas-proxima-reunion.md`** con sugerencias para la próxima reunión (`PS-XXX`)
5. Derivar actores, entidades y reglas de negocio en `02-domain/`
6. Generar casos de uso, historias y criterios en `03-requirements/`
7. Actualizar épicas y features en `04-backlog/`
8. Mantener referencias: `Origen: Entrevista-XXX / H-XXX` en cada artefacto

---

## Matriz de trazabilidad obligatoria

```
Hallazgo (H-XXX)
    ↓
Regla de Negocio (RN-XXX)
    ↓
Entidad (ENT-XXX)
    ↓
Épica (EP-XXX)
    ↓
Feature (FEAT-XXX)
    ↓
Historia (HU-XXX)
    ↓
Criterio de Aceptación (CA-XXX)
    ↓
Implementación (código en src/)
    ↓
Prueba (tests)
```

---

## Regla fundamental

> **Ninguna historia de usuario, caso de uso, API, componente o línea de código debe existir si no puede rastrearse hasta una regla de negocio validada.**

Excepciones permitidas sin nueva documentación:
- Corrección de bugs en código existente (sin cambio funcional)
- Refactorización interna sin cambio de comportamiento
- Actualización de dependencias o configuración de infraestructura

Para cualquier **nueva funcionalidad** o **cambio de comportamiento**, la trazabilidad es obligatoria.

---

## Referencias cruzadas

| Recurso | Ubicación |
|---------|-----------|
| Reglas técnicas C# / API | `.cursor/rules/cSharp-rules.md` |
| Documentación técnica | `README.md` |
| Subagente analista (Cursor) | `.cursor/agents/analista.md` |
| Guía de uso del analista | `knowledge-base/07-ai-context/uso-subagente-analista.md` |
| Convenciones de agente | `.opencode/rules/AGENTS.md` |
| Base de conocimiento | `knowledge-base/` |
