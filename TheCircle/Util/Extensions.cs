using System.Collections.Generic;
using System.Linq;
using TheCircle.Models;

namespace TheCircle.Util
{
    public static class Extensions
    {
        public static Transferencia[] Populate(this IQueryable<Transferencia.BDD> data)
        {
            var transferencias = new List<Transferencia>();
            var usuarios = UserSafe.GetAll();

            foreach (var transferencia in data)
            {
                transferencias.Add(new Transferencia()
                {
                    id = transferencia.id,
                    itemFarmacia = ItemFarmacia.Get( transferencia.itemFarmacia ),
                    solicitante = UserSafe.Get( transferencia.solicitante, usuarios), //UserSafe.Get(usuarios, transferencia.solicitante),
                    cantidad = transferencia.cantidad,
                    fecha = transferencia.fecha,
                    destino = transferencia.destino,
                    cancelado = transferencia.cancelado,
                    autorizadoPor = UserSafe.Get(transferencia.autorizadoPor, usuarios),
                    fechaDespacho = transferencia.fechaDespacho,
                    cantidadDespacho = transferencia.cantidadDespacho,
                    personalDespacho = UserSafe.Get(transferencia.personalDespacho, usuarios),
                    comentarioDespacho = transferencia.comentarioDespacho
                });
            }
            return transferencias.ToArray();
        }
    }
}
