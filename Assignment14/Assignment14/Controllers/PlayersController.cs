using System;
using System.Collections.Generic;
using System.Web.Configuration;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Assignment14.Models;

namespace Assignment14.Controllers
{
    public class PlayersController : Controller
    {
        readonly string conSting = ConfigurationManager.ConnectionStrings["PlayersConStr"].ConnectionString;
        static SqlConnection con;
        static SqlCommand cmd;
        static SqlDataReader srdr;

        // GET: Players
        public ActionResult Index()
        {
            List<Players> players = new List<Players>();
            try
            {
                con = new SqlConnection(conSting);
                cmd = new SqlCommand("select * from Players");
                cmd.Connection = con;
                con.Open();
                srdr = cmd.ExecuteReader();
                while (srdr.Read())
                {
                    players.Add(
                        new Players
                        {
                            PlayerId = int.Parse(srdr["PlayerId"].ToString()),
                            FirstName = (string)srdr["FirstName"],
                            LastName = (string)srdr["LastName"],
                            JerseyNumber = int.Parse(srdr["JerseyNumber"].ToString()),
                            Position = int.Parse(srdr["Position"].ToString()),
                            Team = (string)srdr["Team"]
                        });
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View("Error");
            }
            finally
            {
                con.Close();
            }

            return View(players);
        }

        // GET: Players/Details/5
        public ActionResult Details()
        {
            return View();
        }

        // GET: Players/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Players/Create
        [HttpPost]
        public ActionResult Create(Players player)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    con = new SqlConnection(conSting);
                    cmd = new SqlCommand("INSERT INTO Players (PlayerId,FirstName, LastName, JerseyNumber, Position, Team) VALUES (@playerid,@FirstName, @LastName, @JerseyNumber, @Position, @Team)");
                    cmd.Parameters.AddWithValue("@playerid", player.PlayerId);
                    cmd.Parameters.AddWithValue("@FirstName", player.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", player.LastName);
                    cmd.Parameters.AddWithValue("@JerseyNumber", player.JerseyNumber);
                    cmd.Parameters.AddWithValue("@Position", player.Position);
                    cmd.Parameters.AddWithValue("@Team", player.Team);

                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();

                    TempData["message"] = "Player created successfully!";
                    return RedirectToAction("Index");
                }

                return View(player);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View("Error");
            }
            finally
            {
                con.Close();
            }
        }

        // GET: Players/Edit/5
        public ActionResult Edit(int id)
        {
            // Fetch player details using the id
            Players player = FetchPlayerById(id);
            if (player == null)
            {
                TempData["error"] = "Player not found.";
                return RedirectToAction("Index");
            }

            return View(player);
        }

        // POST: Players/Edit/5
        [HttpPost]
        public ActionResult Edit(Players player)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    con = new SqlConnection(conSting);
                    cmd = new SqlCommand("UPDATE Players SET FirstName = @FirstName, LastName = @LastName, JerseyNumber = @JerseyNumber, Position = @Position, Team = @Team WHERE PlayerId = @PlayerId");
                    cmd.Parameters.AddWithValue("@PlayerId", player.PlayerId);
                    cmd.Parameters.AddWithValue("@FirstName", player.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", player.LastName);
                    cmd.Parameters.AddWithValue("@JerseyNumber", player.JerseyNumber);
                    cmd.Parameters.AddWithValue("@Position", player.Position);
                    cmd.Parameters.AddWithValue("@Team", player.Team);

                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();

                    TempData["message"] = "Player updated successfully!";
                    return RedirectToAction("Index");
                }

                return View(player);
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View("Error");
            }
            finally
            {
                con.Close();
            }
        }

        // GET: Players/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                con = new SqlConnection(conSting);
                cmd = new SqlCommand("DELETE FROM Players WHERE PlayerId = @PlayerId");
                cmd.Parameters.AddWithValue("@PlayerId", id);

                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();

                TempData["message"] = "Player deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return View("Error");
            }
            finally
            {
                con.Close();
            }

            return RedirectToAction("Index");
        }

        // Fetch player details by id
        private Players FetchPlayerById(int id)
        {
            Players player = null;
            try
            {
                con = new SqlConnection(conSting);
                cmd = new SqlCommand("SELECT * FROM Players WHERE PlayerId = @PlayerId");
                cmd.Parameters.AddWithValue("@PlayerId", id);
                cmd.Connection = con;
                con.Open();
                srdr = cmd.ExecuteReader();
                if (srdr.Read())
                {
                    player = new Players
                    {
                        PlayerId = int.Parse(srdr["PlayerId"].ToString()),
                        FirstName = (string)srdr["FirstName"],
                        LastName = (string)srdr["LastName"],
                        JerseyNumber = int.Parse(srdr["JerseyNumber"].ToString()),
                        Position = int.Parse(srdr["Position"].ToString()),
                        Team = (string)srdr["Team"],
                    };
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                
            }
            finally
            {
                con.Close();
            }
            return player;
        }
    }
}
