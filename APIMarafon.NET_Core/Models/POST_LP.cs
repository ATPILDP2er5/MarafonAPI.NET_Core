using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiMarafons.Table; // Пространство имён для ваших моделей
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace ApiMarafons.App_Start
{
    [Route("api/data")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly MARAFON_DBContext _context;

        public DataController(MARAFON_DBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpPost]
        [Route("getdata")]
        public async Task<IActionResult> GetData([FromBody] LoginRequest request)
        {
            var user = await _context.users
                .FirstOrDefaultAsync(u => u.login == request.Login && u.password == request.Password);

            if (user == null)
            {
                return NotFound();
            }

            if (user.TYPEes == 2)
            {
                var zriteliData = await _context.ZRITELI
                    .Where(z => z.UID == user.UID)
                    .Select(z => new
                    {
                        z.UID,
                        z.fam,
                        z.name,
                        z.otch,
                        z.e_mail,
                        z.number_phone,
                        user.TYPEes // Добавляем поле type
                    })
                    .FirstOrDefaultAsync();

                if (zriteliData == null)
                {
                    return NotFound();
                }
                return Ok(zriteliData);
            }
            else if (user.TYPEes == 1)
            {
                var sportsmeniData = await _context.SPORTMENS
                    .Where(s => s.UID == user.UID)
                    .Select(s => new
                    {
                        s.UID,
                        s.fam,
                        s.name,
                        s.otch,
                        s.pol,
                        s.bday,
                        s.strana,
                        user.TYPEes // Добавляем поле type
                    })
                    .FirstOrDefaultAsync();

                if (sportsmeniData == null)
                {
                    return NotFound();
                }
                return Ok(sportsmeniData);
            }
            else
            {
                return BadRequest("Invalid user type");
            }
        }
        
        [HttpPost]
        [Route("EstChel")]
        public async Task<IActionResult> GetLogin([FromBody] LoginRequest request)
        {
            var user = await _context.users
                .AnyAsync(u => u.login == request.Login );
           if(!user)
            {
                return NotFound();
            }
           return Ok(user);
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (_context.users.Any(u => u.login == request.Login))
            {
                return BadRequest("Login already exists.");
            }



       
            await _context.SaveChangesAsync();
            var UIDYu = 1; var UIDYa = 1;
            // Добавление данных в соответствующую таблицу в зависимости от типа пользователя
            switch (request.TYPE.TYPEes)
            {
                case 1: // Спортсмен
                    var athlete = new SPORTMENS
                    {
                        fam = request.Fam,
                        name = request.Name,
                        otch = request.Otch,
                        bday = request.Birthdate,
                        pol = request.Gender,
                        strana = request.Country
                    };
                    _context.SPORTMENS.Add(athlete);
                    await _context.SaveChangesAsync();
                    UIDYa = athlete.UID;
                    break;
                case 2: // Зритель
                    var spectator = new ZRITELI
                    {
                        fam = request.Fam,
                        name = request.Name,
                        otch = request.Otch,
                        e_mail = request.Email,
                        number_phone = request.Phone
                    };
                    _context.ZRITELI.Add(spectator);
                    await _context.SaveChangesAsync();
                    UIDYu = spectator.UID;
                    break;
                    // Добавьте другие типы пользователей, если необходимо
            }
            
            var user = new users
            {
                UID = (request.TYPE.TYPEes == 1) ? UIDYa : UIDYu ,
                login = request.Login,
                password = request.Password,
                TYPEes = request.TYPE.TYPEes
            };
            _context.users.Add(user);

            await _context.SaveChangesAsync();

            return Ok(new { user.ID, user.TYPEes });
        }
    }
    [Route("api/marathons")]
    public class MarathonsController : ControllerBase
    {
        private readonly MARAFON_DBContext _context;

        public MarathonsController(MARAFON_DBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        
        [Route("")]
        public async Task<IActionResult> GetMarathons()
        {
            var marathons = await _context.MARAFON.ToListAsync();
            return Ok(marathons);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetMarathon(int id)
        {
            var marathonsUchasniki = await _context.MARAFON_UCHASTIE.Where(u => u.MUID == id).ToListAsync();
            var allUchastniki = await _context.SPORTMENS.ToListAsync();
            var result = new List<object>();
            List<int?> SUIDmm = new List<int?>();
            if (marathonsUchasniki == null)
            {
                return NotFound();
            }
            foreach (MARAFON_UCHASTIE uCHASTIE in marathonsUchasniki)
            {
                SUIDmm.Add(uCHASTIE.SUID);
            }
            foreach(SPORTMENS sPORTMENS in allUchastniki)
            {
                if(SUIDmm.Contains(sPORTMENS.UID))
                {
                    result.Add(new
                    {
                        fam = sPORTMENS.fam,
                        name = sPORTMENS.name,
                        otch = sPORTMENS.otch,
                        pol = sPORTMENS.pol,
                        bday = sPORTMENS.bday,
                        strana = sPORTMENS.strana
                    });
                    //public string fam { get; set; }
                    //public string name { get; set; }
                    //public string otch { get; set; }
                    //public Nullable<bool> pol { get; set; }
                    //public Nullable<System.DateTime> bday { get; set; }
                    //public string strana { get; set; }
                }
            }
            return Ok(result);


        }
    }
    [Route("api/athletes")]
    public class AthletesController : ControllerBase
    {
        private readonly MARAFON_DBContext _context;

        public AthletesController(MARAFON_DBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetAthlete(int id)
        {
            List<int?> MUIDmm = new List<int?>();
            var marathonsUchasniki = await _context.MARAFON_UCHASTIE.Where(u => u.SUID == id).ToListAsync();
            var result = new object();
            foreach (MARAFON_UCHASTIE mARAFON_UCHASTIE in marathonsUchasniki)
            {
                MUIDmm.Add(mARAFON_UCHASTIE.MUID);
            }
            result = (MUIDmm);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPost]
        [Route("registerM")]
        public async Task<IActionResult> RegisterForMarathon([FromBody] MarathonRegistrationRequest request)
        {
            if (_context.MARAFON_UCHASTIE.Any(m => m.MUID == request.MarathonId && m.SUID == request.AthleteId))
            {
                return BadRequest("Athlete is already registered for this marathon.");
            }

            var registration = new MARAFON_UCHASTIE
            {
                MUID = request.MarathonId,
                SUID = request.AthleteId
            };

            _context.MARAFON_UCHASTIE.Add(registration);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost]
        [Route("unregisterM")]
        public async Task<IActionResult> UnregisterFromMarathon([FromBody] MarathonRegistrationRequest request)
        {
            var registration = await _context.MARAFON_UCHASTIE
                .FirstOrDefaultAsync(m => m.MUID == request.MarathonId && m.SUID == request.AthleteId);

            if (registration == null)
            {
                return NotFound();
            }

            _context.MARAFON_UCHASTIE.Remove(registration);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
    [Route("api/spectators")]
    public class SpectatorsController : ControllerBase
    {
        private readonly MARAFON_DBContext _context;

        public SpectatorsController(MARAFON_DBContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetSpectator(int id)
        {
            var spectator = await _context.ZRITELI.FindAsync(id);
            if (spectator == null)
            {
                return NotFound();
            }
            return Ok(spectator);
        }

        [HttpGet]
        [Route("marathons")]
        public async Task<IActionResult> GetMarathonsWithAthletes()
        {
            var marathons = await _context.MARAFON.ToListAsync();

            var result = marathons.Select(m => new
            {
                m.UID,
                m.NAME,
                m.DLINA,
                m.DATE_START,
                m.VZNOS,
                Athletes = _context.MARAFON_UCHASTIE
                .Where(mu => mu.MUID == m.UID)  // Замените MarafonId на имя столбца с ID марафона в Marafon_uchastie
                .Join(_context.SPORTMENS,
                    mu => mu.SUID,             // Замените AthleteId на имя столбца с ID атлета в Marafon_uchastie
                    u => u.UID,                     // UID - ID атлета в UChastniki
                    (mu, u) => new
                    {
                        u.fam,
                        u.name,
                        u.otch,
                        u.pol,
                        u.bday,
                        u.strana
                    })
                .ToList()

            });

            return Ok(result);
        }
    }


    public class MarathonRegistrationRequest
    {
        public int MarathonId { get; set; }
        public int AthleteId { get; set; }
    }


    public class LoginRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
    public class RegisterRequest
    {
        public int UID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Fam { get; set; } // Фамилия
        public string Name { get; set; } // Имя
        public string Otch { get; set; } // Отчество
        public DateTime? Birthdate { get; set; } // День рождения (для спортсменов)
        public bool Gender { get; set; } // Пол (для спортсменов)
        public string Country { get; set; } // Страна (для спортсменов)
        public string Email { get; set; } // Email (для зрителей)
        public string Phone { get; set; } // Номер телефона (для зрителей)
        public virtual TYPE TYPE { get; set; }
    }

}
