    Manual Técnico - Crowdfunding

 

ELABORADO POR: “Los Zetas” 
Integrantes:    
Sara Luna Maldonado
Maribel Mita Colque
Osvaldo Mérida Corrales
 

1.	Introducción: 
A pedido del cliente de una plataforma web con el nombre de “CrowdFunding” donde un usuario pude registrarse con el fin de poder iniciar una campaña y que puede recibir donaciones por WhatsApp, como un usuario normal y sin necesidad de registrarse tiene la posibilidad de ver los distintos proyectos publicados.
2.	Descripción del proyecto: 
Crowdfunding es una aplicación web que permite a los usuarios crear campañas de recaudación de fondos. Los usuarios registran cuentas, envían proyectos para su revisión y, una vez aprobados, lanzan campañas públicas.
3.	Roles / integrantes:
	Team Lider, SQA líder, GIT Master - Sara Luna Maldonado
	Developer, Arquitecta de software - Maribel Mita Colque
	Developer,Diseñador UI - Osvaldo Mérida Corrales
4.	Arquitectura del software: Se uso el “Modelo Vista Controlador” para el desarrollo de la página web
5.	Requisitos del sistema:
•	Requerimientos de Hardware (mínimo): dispositivo con acceso a internet
•	Requerimientos de Software: (cliente): tener un navegador de confianza, que soporte JavaScript, soporte para Dynamic HTML
•	Requerimientos de Hardware (server/ hosting/BD)
o	Procesador: Se recomienda tener un procesador de múltiples núcleos y alta velocidad para manejar las solicitudes de los clientes de manera eficiente.
o	Memoria (RAM): Cuanta más RAM tenga el servidor, mejor será su capacidad para manejar múltiples solicitudes y ejecutar aplicaciones web complejas. Se recomienda tener al menos 8 GB de RAM, pero la cantidad puede variar según las necesidades del proyecto.
o	Almacenamiento: Se requiere un almacenamiento adecuado para guardar los archivos del sitio web, bases de datos y otros datos relacionados. Se pueden utilizar discos duros o unidades de estado sólido (SSD) para un rendimiento óptimo.
o	Conexión a Internet: Se necesita una conexión a Internet de alta velocidad y confiable para garantizar un acceso rápido y estable al servidor.
•	Requerimientos de Software (server/ hosting/BD)
o	Sistema Operativo: Se puede utilizar una variedad de sistemas operativos para el servidor, como Windows Server, Linux (como Ubuntu, CentOS) o UNIX. La elección del sistema operativo dependerá de la compatibilidad con las tecnologías y aplicaciones utilizadas en el proyecto.
o	Servidor Web: Se necesita un servidor web para entregar las páginas web al cliente. Algunas opciones populares son Apache.
o	Lenguajes de Programación: Dependiendo de las necesidades del proyecto, se pueden utilizar diferentes lenguajes de programación como C#, JavaScript.
o	Base de Datos: Si el proyecto requiere una base de datos, se necesita un sistema de gestión de bases de datos como SQL Server.
6.	Instalación y configuración: 
•	Primero: descargar el proyecto del repositorio de GitHub.
•	Tener Visual Studio instalado, SQL Server v 2019.
•	Tener instalado .Net Core en Visual Studio.  
•	Abrir la aplicación de Visual Studio, ir a “appsettings.json” y cambiar el “Data Source” con la imagen de docker del archivo SQL Server. 
 
7.	 PROCEDIMIENTO DE HOSTEADO / HOSTING (configuración)
•	Se creo un Docker compose donde se encuentra la app y la base de datos dockerizado en DockerHub. 
8.	GIT:
•	Versión final entregada del proyecto.
•	Entrega compilados ejecutables
•	Repositorio: https://github.com/vultines/CrowdFundingFinal
9.	Personalización y configuración: 
Información sobre cómo personalizar y configurar el software según las necesidades del usuario, incluyendo opciones de configuración, parámetros y variables.
•	Configuración de los usuarios:  Se tienen 3 roles en este sistema: 
1.	 Administrador 
2.	Usuario perteneciente a una Organización
3.	Usuario Publico.
•	Parámetros de configuración de cuenta:
1.	Correo electrónico
2.	Nombre completo del usuario registrándose.
3.	Contraseña: esta contraseña debe contar con un carácter especial “@, !” y una letra mayúscula.
•	Cambio de datos del perfil:
1.	En la sección del “Mi perfil” el usuario ¿, ya sea administrador o usuario perteneciente a una organización, podrá cambiar sus datos básicos como: número de teléfono, correo electrónico, nombre completo, foto de perfil.

10.	Seguridad: 
Consideraciones de seguridad y recomendaciones para proteger el software y los datos, incluyendo permisos de acceso, autenticación y prácticas de seguridad recomendadas.
•	Roles y permisos:
Se tiene como principal tres roles.
•Administrador, se encarga de gestionar a los proyectos.
•Usuario perteneciente a Organización, es el que publica y solicita aprobación del administrador para sus publicaciones. 
•Usuario público, Este usuario puede navegar libremente por la página web, con restricciones a opcion  de crear publicaciones de campañas.
10.2.	Autenticación y manejo de contraseña
•	Envió de contraseña autogenerada por correo.
•	Recuperación de contraseña por correo electrónico.

11.	Depuración y solución de problemas: 
Instrucciones sobre cómo identificar y solucionar problemas comunes, mensajes de error y posibles conflictos con otros sistemas o componentes.
Identificación de problemas
•	Mensajes de error
•	Caída del sistema.
Solución de problemas
•	Reiniciar la página
12.	Glosario de términos: 
Un glosario que incluya definiciones de términos técnicos y conceptos utilizados en el manual.
13.	 Herramientas de Implementación:
•	Lenguajes de programación: c#,  javascript,
•	Arquitectura: MVC  


# CrowdFundingFinal

#Links:
Youtube:
https://youtu.be/ppuurnaAyT8

Docker: google drive, ahi se encuentran los archivos
https://drive.google.com/drive/folders/1jf4YAOvEo6YFcchxZeWbQF1K1n-Hwbfh?usp=sharing

