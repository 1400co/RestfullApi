# AI Project Rules — Ocupasalud

Documento maestro para proyectos de desarrollo asistido por IA.
Este archivo define la estructura de artefactos, convenciones y reglas que cualquier agente de
IA (Cursor, Claude Code, OpenCode, Copilot, ChatGPT, Windsurf, Cline, Roo Code, etc.) debe
seguir al trabajar sobre este proyecto.
Este documento es la fuente de verdad para la organización del conocimiento del
proyecto.

## Objetivo

Garantizar que toda la información funcional, técnica y de negocio sea:
- Trazable
- Versionable
- Comprensible para humanos
- Comprensible para IA
- Reutilizable durante todo el ciclo de vida del proyecto

## Principios

### P1. Una sola fuente de verdad
Las reglas de negocio, entidades y glosario son la fuente principal de conocimiento.
La IA nunca debe inventar reglas.
Toda funcionalidad debe estar respaldada por:
- Regla de negocio
- Entidad
- Actor

### P2. Trazabilidad completa
Todo artefacto debe poder rastrearse hasta su origen.

Ejemplo:
```
Entrevista
↓
Hallazgo
↓
Regla de Negocio
↓
Épica
↓
Feature
↓
Historia de Usuario
↓
Código
```

### P3. Markdown como formato oficial
Todos los artefactos deben almacenarse en Markdown.

**Formatos permitidos:**
- `.md`
- `.mmd`
- `.drawio`
- `.json`
- `.yaml`

**Formatos no recomendados:**
- `.docx`
- `.pdf`
- `.xlsx`

Solo se usarán para entregables externos.

### P4. Todo documento debe tener identificadores

Ejemplos:
- `RN-001`
- `RN-002`
- `ACT-001`
- `ENT-001`
- `EP-001`
- `FEAT-001`
- `HU-001`
- `ADR-001`
- `DEC-001`

## Estructura del Proyecto

```
knowledge-base/
├── 01-discovery/
├── 02-domain/
├── 03-requirements/
├── 04-backlog/
├── 05-architecture/
├── 06-decisions/
└── 07-ai-context/
```

---

## 01-DISCOVERY

### Objetivo
Almacenar toda la información obtenida directamente del cliente.
La información de esta carpeta NO representa verdad absoluta.
Representa información pendiente de validación.

### Estructura
```
01-discovery/
├── entrevistas/
├── documentos/
├── procesos/
└── preguntas-abiertas.md
```

### Entrevistas
Formato:
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
- Pregunta 2
```

**Regla**: Las entrevistas NO generan historias de usuario directamente.
Primero deben convertirse en:
- Hallazgos
- Reglas de negocio
- Entidades
- Actores

---

## 02-DOMAIN

### Objetivo
Representar el negocio.
Esta carpeta es la más importante del proyecto.
Toda IA debe consultar esta carpeta antes de generar:
- Código
- Historias
- Casos de uso
- APIs
- Pruebas

### Estructura
```
02-domain/
├── actores.md
├── entidades.md
├── estados.md
├── eventos-negocio.md
├── reglas-negocio.md
└── glosario.md
```

### Actores
Identifican quién interactúa con el sistema.

Formato:
```markdown
# ACT-001 Organizador
## Responsabilidades
- Crear evento
- Modificar evento

## Permisos
- Crear
- Editar
```

### Entidades
Representan conceptos del negocio.

Formato:
```markdown
# ENT-001 Evento
## Descripción
Representa una actividad académica.

## Atributos
- Id
- Nombre
- Estado

## Relaciones
- Participante
- Certificación
```

### Estados
Representan el ciclo de vida de una entidad.

Ejemplo:
```markdown
# Evento
- Borrador
- En Revisión
- Aprobado
- Rechazado
- Publicado
- En Ejecución
- Finalizado
- Cerrado
```

### Eventos de Negocio
Representan hechos importantes.

Formato:
```markdown
# EVN-001 Evento Creado
Descripción:
Un organizador registra un evento.
```

### Reglas de Negocio
Son restricciones obligatorias.

Formato:
```markdown
# RN-001
Título: Evento requiere aprobación.
Descripción: Un evento debe estar aprobado antes de publicarse.
Origen: Entrevista-001
Impacto: Workflow
```

### Glosario
Define términos del negocio.

Formato:
```markdown
# Educación Continua
Definición: Proceso de formación complementaria ofrecido por la universidad.
```

---

## 03-REQUIREMENTS

### Objetivo
Transformar el conocimiento del dominio en funcionalidades.

### Estructura
```
03-requirements/
├── casos-uso/
├── historias/
└── criterios/
```

### Casos de Uso
Formato:
```markdown
# CU-001 Crear Evento
Actor: Organizador

Flujo Principal:
1. Ingresa información
2. Guarda borrador
3. Sistema registra evento

Reglas Relacionadas:
- RN-001
```

### Historias de Usuario
Formato:
```markdown
# HU-001 Crear Evento
Como Organizador
Quiero crear un evento
Para iniciar el proceso de aprobación

## Reglas Relacionadas
- RN-001

## Entidades Relacionadas
- Evento
```

### Criterios de Aceptación
Formato Gherkin.

```markdown
# CA-001
Dado un organizador
Cuando complete los campos obligatorios
Entonces el sistema debe guardar el evento
```

---

## 04-BACKLOG

### Objetivo
Gestionar el trabajo de desarrollo.

### Estructura
```
04-backlog/
├── epicas.md
├── features.md
└── roadmap.md
```

### Épicas
Representan grandes capacidades.

Ejemplo:
```markdown
# EP-001 Gestión de Eventos
Descripción: Administración completa del ciclo de vida de eventos.
```

### Features
Subdividen una épica.

Ejemplo:
```markdown
# FEAT-001 Aprobar Evento
Épica: EP-001
Descripción: Permite aprobar eventos.
```

### Relación Obligatoria
```
Épica
↓
Feature
↓
Historia
```

---

## 05-ARCHITECTURE

### Objetivo
Definir la solución técnica.

### Estructura
```
05-architecture/
├── modelo-dominio.md
├── arquitectura.md
├── integraciones.md
└── adr/
```

### ADR
Architecture Decision Record.

Formato:
```markdown
# ADR-001
Estado: Aceptada
Contexto: Descripción del problema.
Decisión: Descripción de la decisión.
Consecuencias: Impacto esperado.
```

---

## 06-DECISIONS

### Objetivo
Registrar decisiones funcionales y de negocio.

### Formato
```markdown
# DEC-001
Título: Modelo de descuentos.
Fecha: 2026-06-10
Decisión: Los descuentos serán porcentuales.
Justificación: Facilidad de administración.
```

---

## 07-AI-CONTEXT

### Objetivo
Proveer contexto estructurado para agentes IA.

### Estructura
```
07-ai-context/
├── contexto-producto.md
├── prompt-pm.md
├── prompt-analista.md
└── prompt-dev.md
```

---

## Reglas para Agentes IA

Toda IA debe:
1. Leer primero `02-domain/` (reglas-negocio.md, glosario.md, entidades.md)
2. Antes de crear una historia, verificar: Actor existente, Entidad existente, Regla existente
3. Antes de generar código, verificar: Historia existente, Regla asociada, Entidad asociada
4. Nunca crear funcionalidades sin respaldo documental.

## Matriz de Trazabilidad Obligatoria

Toda funcionalidad debe seguir:
```
Hallazgo
↓
Regla de Negocio
↓
Entidad
↓
Épica
↓
Feature
↓
Historia
↓
Criterio de Aceptación
↓
Implementación
↓
Prueba
```

## Regla Fundamental del Proyecto

Ninguna historia de usuario, caso de uso, API, componente o línea de código debe existir si no puede rastrearse hasta una regla de negocio validada.
