using SimpleNote.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNote.Controllers
{
    public class TrashController
    {
        public static bool AddTrash(Trash trash)
        {
            try
            {
                using (var _context = new DBSimpleNoteEntities())
                {
                    _context.Trashes.Add(trash);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static Trash GetTrash(int ID)
        {
            using (var _context = new DBSimpleNoteEntities())
            {
                var trash = (from t in _context.Trashes
                            where t.ID == ID
                            select t).ToList();
                if (trash.Count == 1)
                {
                    return trash[0];
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<Trash> GetListTrash()
        {
            using (var _context = new DBSimpleNoteEntities())
            {
                var trash = (from t in _context.Trashes.AsEnumerable()
                            select t)
                            .Select(x => new Trash
                            {
                                ID = x.ID,
                                description = x.description,
                                dateCreated = x.dateCreated,
                                tags = x.tags,
                                isPinned = x.isPinned
                            }).ToList();

                return trash;
            }
        }

        public static bool DeleteForever(Trash trash)
        {
            using (var _context = new DBSimpleNoteEntities())
            {
                _context.Trashes.Attach(trash);
                _context.Trashes.Remove(trash);
                _context.SaveChanges();
                return true;
            }
        }


        public static bool RefreshTrash()
        {
            using (var _context = new DBSimpleNoteEntities())
            {
                var ID = 0;

                var trashList = (from t in _context.Trashes.AsEnumerable()
                                select t)
                            .Select(x => new Trash
                            {
                                ID = x.ID,
                                description = x.description,
                                dateCreated = x.dateCreated,
                                tags = x.tags,
                                isPinned = x.isPinned
                            }).ToList();



                foreach (Trash trash in trashList)
                {
                    var removeTrash = (from t in _context.Trashes
                                      where t.ID == trash.ID
                                      select t).ToList();
                    trash.ID = ID;
                    _context.Trashes.Remove(removeTrash[0]);
                    _context.Trashes.Add(trash);
                    ID++;
                }


                _context.SaveChanges();
                return true;
            }
        }

        public static bool MoveToFirst(int ID)
        {
            using (var _context = new DBSimpleNoteEntities())
            {
                var newID = 1;

                var trashList = (from t in _context.Trashes.AsEnumerable()
                                select t)
                            .Select(x => new Trash
                            {
                                ID = x.ID,
                                description = x.description,
                                dateCreated = x.dateCreated,
                                tags = x.tags,
                                isPinned = x.isPinned
                            }).ToList();



                foreach (Trash trash in trashList)
                {
                    var removeTrash = (from t in _context.Trashes
                                      where t.ID == trash.ID
                                      select t).ToList();
                    if (trash.ID == ID)
                    {
                        trash.ID = 0;
                        trash.isPinned = true;
                        _context.Trashes.Remove(removeTrash[0]);
                        _context.Trashes.Add(trash);
                        continue;
                    }
                    trash.ID = newID;
                    _context.Trashes.Remove(removeTrash[0]);
                    _context.Trashes.Add(trash);

                    newID++;
                }
                _context.SaveChanges();
                return true;
            }
        }
    }
}
