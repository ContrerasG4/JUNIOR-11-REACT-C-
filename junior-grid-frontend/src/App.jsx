import React, { useEffect, useState } from 'react';
import { getNuevoTablero } from './api/gameApi';
import { Grid } from './components/Grid';
import { Top10View } from './components/Top10View'; 

export default function App() {
  const [vista, setVista] = useState('grilla'); // 'grilla' o 'top10'
  const [tablero, setTablero] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const cargarTablero = async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await getNuevoTablero();
      setTablero(data);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (vista === 'grilla') {
      cargarTablero();
    }
  }, [vista]);

  return (
    <div className="min-h-screen bg-slate-900 text-white flex flex-col items-center py-8 px-4">
      <header className="text-center mb-6">
        <h1 className="text-4xl md:text-5xl font-black text-red-600 tracking-wider uppercase mb-2">
          Junior Grid & Stats
        </h1>
        <p className="text-slate-400 text-sm md:text-base">
          Elige tu modo de juego favorito
        </p>
      </header>

      {/* Botones de navegación entre modos */}
      <div className="flex gap-4 mb-8">
        <button
          onClick={() => setVista('grilla')}
          className={`px-5 py-2.5 font-bold rounded-lg shadow-lg transition ${
            vista === 'grilla' ? 'bg-red-600 text-white' : 'bg-slate-800 text-slate-400 hover:bg-slate-700'
          }`}
        >
          🧩 Modo Grilla (3x3)
        </button>
        <button
          onClick={() => setVista('top10')}
          className={`px-5 py-2.5 font-bold rounded-lg shadow-lg transition ${
            vista === 'top10' ? 'bg-yellow-500 text-slate-900' : 'bg-slate-800 text-slate-400 hover:bg-slate-700'
          }`}
        >
          🏆 Modo Top 10
        </button>
      </div>

      {/* Renderizado condicional según la vista seleccionada */}
      {vista === 'grilla' ? (
        <>
          {loading && (
            <div className="flex flex-col items-center justify-center my-12">
              <div className="w-12 h-12 border-4 border-red-600 border-t-transparent rounded-full animate-spin"></div>
              <p className="mt-4 text-slate-300">Generando tablero aleatorio...</p>
            </div>
          )}

          {error && (
            <div className="bg-red-500/20 border border-red-500 text-red-200 p-4 rounded-xl text-center max-w-md my-8">
              <p className="font-semibold">{error}</p>
              <button
                onClick={cargarTablero}
                className="mt-4 px-4 py-2 bg-red-600 text-white rounded-lg hover:bg-red-700 transition"
              >
                Reintentar
              </button>
            </div>
          )}

          {!loading && !error && tablero && (
            <main className="w-full">
              <div className="flex justify-center mb-6">
                <button
                  onClick={cargarTablero}
                  className="px-5 py-2.5 bg-red-600 hover:bg-red-700 font-bold rounded-lg shadow-lg transition flex items-center gap-2"
                >
                  🔄 Nuevo Tablero
                </button>
              </div>
              <Grid tablero={tablero} />
            </main>
          )}
        </>
      ) : (
      
        <Top10View />
      )}
    </div>
  );
}