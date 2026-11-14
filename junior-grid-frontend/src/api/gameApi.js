const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000';
const API_KEY = import.meta.env.VITE_API_KEY || 'Junior11-teomaximoidolo'; 

let sessionToken = null;

const obtenerToken = async () => {
  if (sessionToken) return sessionToken;

  const response = await fetch(`${API_URL}/api/auth/sesion`, {
    method: 'POST',
    headers: {
      'X-API-KEY': API_KEY
    }
  });

  if (!response.ok) {
    throw new Error('No se pudo iniciar la sesión anónima');
  }

  const data = await response.json();
  sessionToken = data.token; // Guardamos el JWT
  return sessionToken;
};

const getHeaders = async (isJson = false) => {
  const token = await obtenerToken(); 
  
  const headers = {
    'Authorization': `Bearer ${token}` 
  };
  
  if (isJson) {
    headers['Content-Type'] = 'application/json';
  }
  
  return headers;
};


export const getNuevoTablero = async () => {
  const response = await fetch(`${API_URL}/api/GameGrid/nuevo-tablero`, {
    headers: await getHeaders() 
  });
  
  if (!response.ok) {
    const errorData = await response.json().catch(() => ({}));
    throw new Error(errorData.mensaje || 'Error al obtener el tablero');
  }
  return await response.json();
};

export const validarJugador = async (data) => {
  const response = await fetch(`${API_URL}/api/GameGrid/validar`, {
    method: 'POST',
    headers: await getHeaders(true), 
    body: JSON.stringify(data),
  });
  
  if (!response.ok) {
    const errorData = await response.json().catch(() => ({}));
    throw new Error(errorData.mensaje || 'Error al validar el jugador');
  }
  return await response.json();
};

export const getTop10Reto = async () => {
 const response = await fetch(`${API_URL}/api/top10/reto`, {
    headers: await getHeaders() 
  });
  
  if (!response.ok) {
    const errorData = await response.json().catch(() => ({}));
    throw new Error(errorData.mensaje || 'Error al obtener el reto de Top 10');
  }
  return await response.json();
};