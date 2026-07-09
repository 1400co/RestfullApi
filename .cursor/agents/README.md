# Subagentes de Cursor — RestfullApi

Subagentes especializados del proyecto. Cursor los carga desde `.cursor/agents/` y el agente principal puede delegarles tareas según la `description` de cada archivo.

## Cadena de trabajo

```
analista → arquitecto (opcional) → desarrollador → verificador
```

| Fase | Subagente | Ámbito |
|------|-----------|--------|
| Requerimientos | `analista` | `knowledge-base/` |
| Decisiones técnicas | `arquitecto` | `knowledge-base/05-architecture/` |
| Implementación | `desarrollador` | `src/` |
| Validación | `verificador` | Solo lectura |

## Subagentes disponibles

| Subagente | Archivo | Uso |
|-----------|---------|-----|
| **analista** | `analista.md` | Entrevistas → requerimientos en `knowledge-base/` |
| **arquitecto** | `arquitecto.md` | Consultoría técnica (pros/contras) y ADRs |
| **desarrollador** | `desarrollador.md` | HU/CA → código en `src/` |
| **verificador** | `verificador.md` | Validar implementación vs requerimientos |

## Invocación rápida

```
/analista Procesa knowledge-base/01-discovery/entrevistas/entrevista-001.md

/arquitecto ¿Redis o in-memory cache para este proyecto? Presenta pros y contras.

/desarrollador Implementa HU-001 y sus criterios CA-001 según cSharp-rules.md

/verificador Confirma que HU-001 y CA-001 están correctamente implementados
```

## Documentación detallada

| Subagente | Guía |
|-----------|------|
| analista | [`uso-subagente-analista.md`](../knowledge-base/07-ai-context/uso-subagente-analista.md) |
| arquitecto, desarrollador, verificador | [`uso-subagentes.md`](../knowledge-base/07-ai-context/uso-subagentes.md) |
