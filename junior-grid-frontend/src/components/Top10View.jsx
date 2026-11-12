import React, { useState, useEffect } from 'react';

export const Top10View = () => {
  const [reto, setReto] = useState(null);
  const [aciertos, setAciertos] = useState(Array(10).fill(null));
  const [inputNombre, setInputNombre] = useState('');
  const [loading, setLoading] = useState(true);

  const cargarTop10 = async () => {
    setLoading(true);
    setAciertos(Array(10).fill(null)); // Reiniciamos los aciertos
    try {
      const response = await fetch('http://localhost:5000/api/top10/reto');
      if (!response.ok) throw new Error('Error al obtener el reto Top 10');
      const data = await response.json();
      setReto(data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    cargarTop10();
  }, []);

  const handleValidar = (e) => {
    e.preventDefault();
    const textoIngresado = inputNombre.trim().toLowerCase();
    if (!textoIngresado || !reto) return;

    // Búsqueda flexible con .includes() para que detecte al jugador con solo escribir parte del nombre (ej: "teo")
    const indexEncontrado = reto.posiciones.findIndex((pos, idx) => 
      !aciertos[idx] && pos.nombre.toLowerCase().includes(textoIngresado)
    );

    if (indexEncontrado !== -1) {
      const nuevosAciertos = [...aciertos];
      nuevosAciertos[indexEncontrado] = reto.posiciones[indexEncontrado].nombre;
      setAciertos(nuevosAciertos);
    }

    setInputNombre('');
  };

  if (loading) {
    return (
      <div className="flex flex-col items-center justify-center my-12">
        <div className="w-10 h-10 border-4 border-yellow-500 border-t-transparent rounded-full animate-spin"></div>
        <p className="mt-4 text-slate-400 text-sm">Generando reto aleatorio...</p>
      </div>
    );
  }

  return (
    <div className="w-full max-w-md mx-auto p-4 text-center">
      <h2 className="text-2xl font-black text-yellow-400 uppercase tracking-wide mb-1">
        JUNIOR 11 – TOP 10
      </h2>
      <p className="text-slate-300 text-sm mb-4 font-semibold">
        {reto?.tituloReto}
      </p>

      {/* Botón para cambiar de Top 10 */}
      <div className="mb-6">
        <button
          onClick={cargarTop10}
          className="px-4 py-2 bg-yellow-500 hover:bg-yellow-600 text-slate-900 font-bold rounded-lg text-xs transition shadow-md"
        >
          🔄 Cambiar de Reto Top 10
        </button>
      </div>

      {/* Lista Vertical de las 10 casillas */}
      <div className="flex flex-col gap-2.5 mb-6">
        {reto?.posiciones && reto.posiciones.length > 0 ? (
          reto.posiciones.map((pos, index) => (
            <div
              key={index}
              className="flex items-center bg-slate-800/90 border border-slate-700 rounded-lg p-3 text-white shadow-md justify-between"
            >
              <div className="flex items-center gap-3">
                <span className="w-6 font-extrabold text-slate-400">{pos.posicion}.</span>
                <span className="text-xs bg-slate-700 px-2 py-1 rounded text-slate-300 font-semibold">
                  {pos.nacionalidad}
                </span>
                <span className="text-xs text-slate-400">{pos.decada}</span>
              </div>

              <div className="font-bold">
                {aciertos[index] ? (
                  <span className="text-green-400">{aciertos[index]}</span>
                ) : (
                  <span className="text-slate-600 tracking-widest text-sm">••••••••••••</span>
                )}
              </div>
            </div>
          ))
        ) : (
          <p className="text-slate-400 text-sm">Cargando posiciones...</p>
        )}
      </div>

      {/* Input Inferior para adivinar */}
      <form onSubmit={handleValidar} className="flex gap-2">
        <input
          type="text"
          placeholder="Escribe el futbolista aquí..."
          value={inputNombre}
          onChange={(e) => setInputNombre(e.target.value)}
          className="flex-1 bg-slate-800 border border-slate-700 rounded-lg px-4 py-3 text-white outline-none focus:border-yellow-500 text-sm"
          autoFocus
        />
        <button
          type="submit"
          className="bg-yellow-500 hover:bg-yellow-600 text-slate-900 font-bold px-4 py-3 rounded-lg transition"
        >
          🏁
        </button>
      </form>
    </div>
  );
};