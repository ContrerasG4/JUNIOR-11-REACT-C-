import React, { useState } from 'react';

export const CellModal = ({ isOpen, onClose, onSelectJugador, fila, columna }) => {
  const [nombre, setNombre] = useState('');

  if (!isOpen) return null;

  const handleSubmit = (e) => {
    e.preventDefault();
    if (!nombre.trim()) return;
    onSelectJugador(nombre.trim());
    setNombre('');
  };

  return (
    <div className="fixed inset-0 bg-black/60 flex items-center justify-center z-50 p-4">
      <div className="bg-white rounded-xl max-w-md w-full p-6 shadow-2xl relative text-gray-800">
        <h3 className="text-xl font-bold mb-2 text-red-700">Escribe el Jugador</h3>
        <p className="text-sm text-gray-600 mb-1">
          Fila: <span className="font-semibold">{fila?.etiqueta}</span>
        </p>
        <p className="text-sm text-gray-600 mb-4">
          Columna: <span className="font-semibold">{columna?.etiqueta}</span>
        </p>

        <form onSubmit={handleSubmit} className="flex flex-col gap-4">
          <input
            type="text"
            placeholder="Ej. Carlos Bacca, Teofilo Gutierrez..."
            value={nombre}
            onChange={(e) => setNombre(e.target.value)}
            className="w-full border border-gray-300 rounded-lg p-3 outline-none focus:border-red-600"
            autoFocus
            required
          />

          <div className="flex justify-end gap-2">
            <button
              type="button"
              onClick={onClose}
              className="px-4 py-2 bg-gray-200 text-gray-700 rounded-lg hover:bg-gray-300 transition"
            >
              Cancelar
            </button>
            <button
              type="submit"
              className="px-4 py-2 bg-red-600 text-white font-semibold rounded-lg hover:bg-red-700 transition"
            >
              Validar Acierto
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};