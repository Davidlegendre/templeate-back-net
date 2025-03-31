# .NET Project Template

![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)

Este es un template de proyecto en .NET diseñado para acelerar el desarrollo y estandarizar la estructura del código.

## 🚀 Características

- Arquitectura basada en **Clean Architecture**.
- Separación de responsabilidades en capas: **Application, Domain, Infrastructure, RepoSQL**.
- Configuración de inyección de dependencias.
- Manejo de configuración con `.ini`.

## 📂 Estructura del Proyecto

```
📦 templeate-dotnet-main
├── 📂 Application         # Lógica de aplicación y casos de uso
├── 📂 Domain             # Entidades y reglas de negocio
├── 📂 Infrastructure     # Implementaciones de servicios y acceso a datos
├── 📂 RepoSQL           # Capa de acceso a base de datos
├── 📂 template-dotnet-main # Configuración de la plantilla
├── .gitignore            # Configuración de archivos ignorados por Git
├── LICENSE               # Licencia del proyecto
```

## 🎯 Instalación y Uso

### **1. Instalar la plantilla en .NET CLI**

```sh
dotnet new --install ./
```

### **2. Crear un nuevo proyecto con la plantilla**

```sh
dotnet new miwebapi -o MiNuevoProyecto
```

### **3. Ejecutar el proyecto**

```sh
cd MiNuevoProyecto
dotnet run
```
## 🛠 Personalización

Puedes modificar `template.json` en `.template.config/` para cambiar:
- El nombre del paquete de la plantilla (`identity`).
- El comando de creación (`shortName`).
- Variables de reemplazo (`sourceName`).

## 🤝 Contribuciones

¡Las contribuciones son bienvenidas! Puedes abrir un **issue** o un **pull request** para mejorar la plantilla.

## 📜 Licencia

Este proyecto está bajo la licencia MIT. Consulta el archivo `LICENSE` para más detalles.

