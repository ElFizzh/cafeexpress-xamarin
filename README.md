# CaféExpress — App de gestión de pedidos para cafetería

Aplicación móvil desarrollada con **Xamarin.Forms** aplicando el patrón de diseño **MVVM**
y control de acceso basado en roles (**RBAC**), como evidencia académica.

## Estructura del proyecto (MVVM)

```
CafeExpress/
├─ App.xaml / App.xaml.cs   → Arranque de la aplicación
├─ Models/                  → Entidades: Usuario, Producto, Pedido, roles y permisos
├─ Services/                → AuthService (autenticación), RbacService (matriz RBAC), DataService (datos)
├─ ViewModels/              → Lógica de presentación (propiedades y comandos para data binding)
└─ Views/                   → Pantallas XAML con su código asociado
```

## Usuarios de demostración

| Rol           | Usuario   | Contraseña   |
|---------------|-----------|--------------|
| Administrador | admin     | admin123     |
| Empleado      | empleado  | empleado123  |
| Cliente       | cliente   | cliente123   |

## Cómo compilar

1. Instalar **Visual Studio 2022 Community** con la carga de trabajo *"Desarrollo móvil con .NET"* (Xamarin).
2. Crear un proyecto nuevo: **Aplicación Xamarin.Forms** → nombre `CafeExpress` → plantilla *En blanco* → plataforma Android.
3. En el proyecto compartido `CafeExpress`, reemplazar `App.xaml` / `App.xaml.cs` y copiar las carpetas `Models`, `Services`, `ViewModels` y `Views` de este repositorio.
4. Establecer `CafeExpress.Android` como proyecto de inicio y ejecutar (F5) en un emulador o dispositivo físico con Android 8.0+.

## Seguridad

- Contraseñas almacenadas con hash **SHA-256** (nunca en texto plano).
- Permisos definidos por rol en una matriz RBAC (`Services/RbacService.cs`).
- Doble validación: las opciones no permitidas se ocultan en la interfaz **y** cada operación
  vuelve a verificar el permiso en la lógica antes de ejecutarse.
