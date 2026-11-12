const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000';

export const getNuevoTablero = async () => {
  const response = await fetch(`${API_URL}/api/GameGrid/nuevo-tablero`);
  
  if (!response.ok) {
    const errorData = await response.json().catch(() => ({}));
    throw new Error(errorData.mensaje || 'Error al obtener el tablero');
  }
  return await response.json();
};

export const validarJugador = async (data) => {
  const response = await fetch(`${API_URL}/api/GameGrid/validar`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(data),
  });
  
  if (!response.ok) {
    const errorData = await response.json().catch(() => ({}));
    throw new Error(errorData.mensaje || 'Error al validar el jugador');
  }
  return await response.json();
};

export const getTop10Reto = async () => {
  const response = await fetch(`${API_URL}/api/top10/reto`);
  
  if (!response.ok) {
    const errorData = await response.json().catch(() => ({}));
    throw new Error(errorData.mensaje || 'Error al obtener el reto de Top 10');
  }
  return await response.json();
};