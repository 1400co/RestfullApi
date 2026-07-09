# Subagentes de Cursor — RestfullApi

Subagentes especializados del proyecto. Cursor los carga desde `.cursor/agents/` y el agente principal puede delegarles tareas según la `description` de cada archivo.

Documentación detallada: [`knowledge-base/07-ai-context/uso-subagente-analista.md`](../knowledge-base/07-ai-context/uso-subagente-analista.md)

## Subagentes disponibles

| Subagente | Archivo | Uso |
|-----------|---------|-----|
| **analista** | `analista.md` | Análisis de entrevistas → requerimientos en `knowledge-base/` |

## Invocación rápida

```
/analista Procesa knowledge-base/01-discovery/entrevistas/entrevista-001.md
```

O en lenguaje natural:

```
Usa el subagente analista para procesar la entrevista en knowledge-base/01-discovery/entrevistas/entrevista-001.md
```
