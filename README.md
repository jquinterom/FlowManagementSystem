# Flow Management System

## Requisitos

- C#
- ASP.NET Core
- MongoDB

## Instalación

1. Clonar el repositorio
2. Abrir el proyecto en Visual Studio
3. Configurar la conexión a MongoDB creando un archivo .env en el proyecto API con la siguiente configuración:

```
MONGODB_CONNECTION_STRING=mongodb://localhost:27017
MONGODB_DATABASE_NAME=flow_management
MONGODB_COLLECTION_NAME=flows
```

4. Ejecutar el proyecto

## Uso

### Flow

#### Crear un nuevo flujo

Puede ingresar a la url http://localhost:<puerto>/api/flows para crear un nuevo flujo. Recuerda cambiar el puerto según la ejecución del proyecto.

POST /api/flows

```json
{
  "name": "Flow 1",
  "description": "Descripción del flujo",
  "purpose": "Purpose of the flow"
}
```

### Step

#### Crear un nuevo paso

Puede ingresar a la url http://localhost:<puerto>/api/steps para crear un nuevo paso.

POST /api/steps

```json
{
  "code": "STP-0001",
  "name": "Step 1",
  "description": "Descripción del paso",
  "order": 1,
  "isParallel": false,
  "actionType": "DocumentValidation",
  "inputs": [
    {
      "fieldCode": "F-0001",
      "isRequired": true
    }
  ],
  "outputs": [
    {
      "fieldCode": "F-0002"
    }
  ]
}
```

#### Agregar pasos a un flujo

Puede ingresar a la url http://localhost:<puerto>/api/flows/<idFlow>/steps para agregar pasos a un flujo.

POST /api/flows/<idFlow>/steps

```json
["STP-0001", "STP-0002"]
```

#### Obtener pasos de un flujo

Puede ingresar a la url http://localhost:<puerto>/api/flows/<idFlow>/steps para obtener los pasos de un flujo.

GET /api/flows/<idFlow>/steps

### Field

#### Crear un nuevo campo

Puede ingresar a la url http://localhost:<puerto>/api/fields para crear un nuevo campo.

POST /api/fields

```json
{
  "code": "F-0001",
  "name": "Field 1",
  "description": "Descripción del campo",
  "dataType": "string"
}
```

#### Agregar campos a un paso

Puede ingresar a la url http://localhost:<puerto>/api/steps/<idStep>/fields para agregar campos a un paso.

POST /api/steps/<idStep>/fields

```json
["F-0001", "F-0002"]
```

#### Obtener campos de un paso

Puede ingresar a la url http://localhost:<puerto>/api/steps/<idStep>/fields para obtener los campos de un paso.

GET /api/steps/<idStep>/fields

### Flow Execution

#### Iniciar una ejecución de un flujo

Puede ingresar a la url http://localhost:<puerto>/api/flows/execute/<idFlow> para iniciar una ejecución de un flujo.

POST /api/flows/execute/<idFlow>

```json
{
  "inputs": {
    "F-0001": "Valor del campo F-0001",
    "F-0002": "Valor del campo F-0002"
  }
}
```

#### Obtener la ejecución de un flujo

Puede ingresar a la url http://localhost:<puerto>/api/flows/execute/<executionId> para obtener la ejecución de un flujo.

GET /api/flows/<idFlow>/execute/<idExecution>
