import React, { useState } from 'react';
import { CellModal } from './CellModal';
import { validarJugador } from '../api/gameApi';

export const Grid = ({ tablero }) => {
  const [casillas, setCasillas] = useState(
    Array(3).fill(null).map(() => Array(3).fill(null))
  );
  const [modalState, setModalState] = useState({
    isOpen: false,
    filaIndex: null,
    colIndex: null,
  });

  const handleOpenModal = (rIndex, cIndex) => {
    if (casillas[rIndex][cIndex]?.esValido) return;

    setModalState({
      isOpen: true,
      filaIndex: rIndex,
      colIndex: cIndex,
    });
  };

const handleValidar = async (nombreIngresado) => {
    const { filaIndex, colIndex } = modalState;
    const criterioFila = tablero.filas[filaIndex];
    const criterioCol = tablero.columnas[colIndex];

    try {
      const resultado = await validarJugador({
        nombreJugador: nombreIngresado, // <--- Mandando el nombre limpio
        criterioFilaId: criterioFila.id,
        criterioColumnaId: criterioCol.id,
      });

      const nuevasCasillas = [...casillas];
      nuevasCasillas[filaIndex][colIndex] = resultado;
      setCasillas(nuevasCasillas);
    } catch (err) {
      alert('Error al validar la casilla: ' + err.message);
    } finally {
      setModalState({ isOpen: false, filaIndex: null, colIndex: null });
    }
  };
  
  return (
    <div className="w-full max-w-3xl mx-auto p-4">
      <div className="grid grid-cols-4 gap-2 text-center font-bold">
        <div className="p-3"></div>

        {tablero.columnas.map((col) => (
          <div
            key={col.id}
            className="bg-red-800 text-white p-3 rounded-lg flex items-center justify-center text-sm md:text-base shadow"
          >
            {col.etiqueta}
          </div>
        ))}

        {tablero.filas.map((fila, rIndex) => (
          <React.Fragment key={fila.id}>
            <div className="bg-red-800 text-white p-3 rounded-lg flex items-center justify-center text-sm md:text-base shadow">
              {fila.etiqueta}
            </div>

            {tablero.columnas.map((col, cIndex) => {
              const estadoCasilla = casillas[rIndex][cIndex];

              return (
                <button
                  key={cIndex}
                  onClick={() => handleOpenModal(rIndex, cIndex)}
                  className={`h-24 md:h-32 border-2 rounded-lg flex flex-col items-center justify-center p-2 transition font-medium shadow-sm ${
                    estadoCasilla?.esValido
                      ? 'bg-green-100 border-green-500 text-green-900'
                      : estadoCasilla?.esValido === false
                      ? 'bg-red-100 border-red-400 text-red-900'
                      : 'bg-white border-gray-200 hover:border-red-500 hover:shadow-md'
                  }`}
                >
                  {estadoCasilla ? (
                    <>
                      <span className="font-bold text-base md:text-lg">{estadoCasilla.jugador}</span>
                      <span className="text-xs mt-1">{estadoCasilla.mensaje}</span>
                    </>
                  ) : (
                    <span className="text-gray-400 text-2xl font-light">+</span>
                  )}
                </button>
              );
            })}
          </React.Fragment>
        ))}
      </div>

      <CellModal
        isOpen={modalState.isOpen}
        onClose={() => setModalState({ isOpen: false, filaIndex: null, colIndex: null })}
        onSelectJugador={handleValidar}
        fila={modalState.filaIndex !== null ? tablero.filas[modalState.filaIndex] : null}
        columna={modalState.colIndex !== null ? tablero.columnas[modalState.colIndex] : null}
      />
    </div>
  );
};
