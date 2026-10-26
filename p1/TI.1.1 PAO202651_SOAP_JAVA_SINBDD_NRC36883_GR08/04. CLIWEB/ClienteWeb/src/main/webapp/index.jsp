<%@page contentType="text/html" pageEncoding="UTF-8"%>
<!DOCTYPE html>
<html lang="es">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Login - Monsters Inc Conversor SOAP GR08</title>
    <style>
        * {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
        }
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: radial-gradient(circle at top, #1e293b, #0f172a 80%);
            color: #f8fafc;
            display: flex;
            justify-content: center;
            align-items: center;
            min-height: 100vh;
            padding: 20px;
        }
        .login-card {
            display: flex;
            background-color: #151b28;
            border: 1px solid #334155;
            border-radius: 12px;
            overflow: hidden;
            box-shadow: 0 20px 40px rgba(0, 0, 0, 0.6);
            max-width: 740px;
            width: 100%;
        }
        .form-section {
            flex: 1;
            padding: 40px 35px;
            display: flex;
            flex-direction: column;
            justify-content: center;
        }
        .badge {
            display: inline-block;
            background-color: #2563eb;
            color: white;
            font-size: 0.75rem;
            font-weight: 700;
            text-transform: uppercase;
            padding: 4px 10px;
            border-radius: 20px;
            margin-bottom: 12px;
            letter-spacing: 0.5px;
            width: fit-content;
        }
        h2 {
            font-size: 1.6rem;
            font-weight: 700;
            margin-bottom: 8px;
            color: #ffffff;
        }
        p.subtitle {
            color: #94a3b8;
            font-size: 0.88rem;
            margin-bottom: 25px;
        }
        .input-group {
            margin-bottom: 18px;
        }
        label {
            display: block;
            font-size: 0.85rem;
            font-weight: 600;
            margin-bottom: 6px;
            color: #cbd5e1;
        }
        input[type="text"], input[type="password"] {
            width: 100%;
            background-color: #1e293b;
            border: 1px solid #475569;
            border-radius: 6px;
            color: #ffffff;
            padding: 12px 14px;
            font-size: 0.95rem;
            outline: none;
            transition: border-color 0.2s;
        }
        input[type="text"]:focus, input[type="password"]:focus {
            border-color: #3b82f6;
            box-shadow: 0 0 0 2px rgba(59, 130, 246, 0.2);
        }
        button.btn-login {
            width: 100%;
            background-color: #3b82f6;
            color: white;
            border: none;
            border-radius: 6px;
            padding: 13px;
            font-size: 1rem;
            font-weight: 700;
            cursor: pointer;
            margin-top: 10px;
            transition: background-color 0.2s, transform 0.1s;
        }
        button.btn-login:hover {
            background-color: #2563eb;
        }
        button.btn-login:active {
            transform: scale(0.99);
        }
        .error-msg {
            background-color: rgba(239, 68, 68, 0.15);
            border-left: 4px solid #ef4444;
            color: #fca5a5;
            padding: 12px;
            border-radius: 4px;
            font-size: 0.85rem;
            margin-top: 18px;
            font-weight: 500;
        }
        .image-section {
            width: 280px;
            background-color: #0d121c;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 20px;
            border-left: 1px solid #1e293b;
        }
        .image-section img {
            width: 100%;
            height: auto;
            max-height: 380px;
            object-fit: cover;
            border-radius: 8px;
            box-shadow: 0 10px 25px rgba(0, 0, 0, 0.5);
        }
        @media (max-width: 650px) {
            .login-card {
                flex-direction: column-reverse;
            }
            .image-section {
                width: 100%;
                border-left: none;
                border-bottom: 1px solid #1e293b;
            }
            .image-section img {
                max-height: 200px;
                object-fit: contain;
            }
        }
    </style>
</head>
<body>
    <div class="login-card">
        <div class="form-section">
            <span class="badge">SOA Web Service &bull; GR08</span>
            <h2>Monsters Inc. Conversor</h2>
            <p class="subtitle">Ingresa tus credenciales para acceder</p>

            <form method="POST" action="LoginServlet">
                <div class="input-group">
                    <label for="txtUsuario">Usuario:</label>
                    <input type="text" id="txtUsuario" name="txtUsuario" placeholder="monster" required autofocus>
                </div>
                <div class="input-group">
                    <label for="txtClave">Contraseña:</label>
                    <input type="password" id="txtClave" name="txtClave" placeholder="monster9" required>
                </div>
                <button type="submit" class="btn-login">INICIAR SESIÓN</button>
            </form>

            <% if (request.getAttribute("error") != null) { %>
                <div class="error-msg">
                    <strong>No se pudo iniciar sesión</strong><br>
                    <%= request.getAttribute("error") %>
                </div>
            <% } %>
        </div>

        <div class="image-section">
            <img src="Imagenes/monsterlogin.jpg" alt="James P. Sullivan - Monsters Inc" title="James P. Sullivan">
        </div>
    </div>
</body>
</html>
