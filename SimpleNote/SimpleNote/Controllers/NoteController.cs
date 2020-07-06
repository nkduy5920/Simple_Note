using SimpleNote.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleNote.Controllers
{
    public class NoteController
    {
        public static bool AddNote(Note note)
        {
            try
            {
                using (var _context = new DBSimpleNoteEntities())
                {
                    _context.Notes.AddOrUpdate(note);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static Note GetNote(int ID)
        {
            using (var _context = new DBSimpleNoteEntities())
            {
                var note = (from n in _context.Notes
                            where n.ID == ID
                            select n).ToList();
                if (note.Count == 1)
                {
                    return note[0]; 
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<Note> GetListNote()
        {
            using (var _context = new DBSimpleNoteEntities())
            {
                var note = (from n in _context.Notes.AsEnumerable()
                            select n)
                            .Select(x => new Note
                            {
                                ID = x.ID,
                                description = x.description,
                                dateCreated = x.dateCreated,
                                tags = x.tags,
                                isPinned = x.isPinned
                            }).ToList();

                return note;
            }
        }


        public static bool UpdateNote(Note note)
        {
            try
            {
                using (var _context = new DBSimpleNoteEntities())
                {
                    _context.Notes.AddOrUpdate(note);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }


        public static bool RemoveNote(Note note)
        {
            using (var _context = new DBSimpleNoteEntities())
            {
                _context.Notes.Attach(note);
                _context.Notes.Remove(note);
                _context.SaveChanges();
                return true;
            }
        }


        public static bool RefreshNote()
        {
            using (var _context = new DBSimpleNoteEntities())
            {
                var ID = 0;

                var noteList = (from n in _context.Notes.AsEnumerable()
                            select n)
                            .Select(x => new Note
                            {
                                ID = x.ID,
                                description = x.description,
                                dateCreated = x.dateCreated,
                                tags = x.tags,
                                isPinned = x.isPinned
                            }).ToList();

                

                foreach (Note note in noteList)
                {
                    var removeNote = (from n in _context.Notes
                                      where n.ID == note.ID
                                      select n).ToList();
                    note.ID = ID;
                    _context.Notes.Remove(removeNote[0]);
                    _context.Notes.Add(note);
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

                var noteList = (from n in _context.Notes.AsEnumerable()
                                select n)
                            .Select(x => new Note
                            {
                                ID = x.ID,
                                description = x.description,
                                dateCreated = x.dateCreated,
                                tags = x.tags,
                                isPinned = x.isPinned
                            }).ToList();



                foreach (Note note in noteList)
                {
                    var removeNote = (from n in _context.Notes
                                      where n.ID == note.ID
                                      select n).ToList();
                    if (note.ID == ID)
                    {
                        note.ID = 0;
                        note.isPinned = true;
                        _context.Notes.Remove(removeNote[0]);
                        _context.Notes.Add(note);
                        continue;
                    }
                    note.ID = newID;
                    _context.Notes.Remove(removeNote[0]);
                    _context.Notes.Add(note);
                    
                    newID++;
                }
                _context.SaveChanges();
                return true;
            }
        }

        public static bool MoveToLast(int ID)
        {
            using (var _context = new DBSimpleNoteEntities())
            {
                var newID = 0;

                var noteList = (from n in _context.Notes.AsEnumerable()
                                select n)
                            .Select(x => new Note
                            {
                                ID = x.ID,
                                description = x.description,
                                dateCreated = x.dateCreated,
                                tags = x.tags,
                                isPinned = x.isPinned
                            }).ToList();

                foreach (Note note in noteList)
                {
                    var removeNote = (from n in _context.Notes
                                      where n.ID == note.ID
                                      select n).ToList();
                    if (note.ID == ID)
                    {
                        note.ID = NoteController.GetListNote().Count - 1;
                        note.isPinned = false;
                        _context.Notes.Remove(removeNote[0]);
                        _context.Notes.Add(note);
                        continue;
                    }
                    note.ID = newID;
                    _context.Notes.Remove(removeNote[0]);
                    _context.Notes.Add(note);
                    RefreshNote();
                    newID++;
                }

                _context.SaveChanges();
                return true;
            }
        }
    }
}
