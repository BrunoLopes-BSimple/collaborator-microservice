using Domain.Models;
using Infrastructure.DataModel;
using Domain.IRepository;
using Microsoft.EntityFrameworkCore;
using Domain.Interfaces;
using AutoMapper;

namespace Infrastructure.Repositories;
public class HolidayPlanRepositoryEF : GenericRepository<IHolidayPlan, HolidayPlanDataModel>, IHolidayPlanRepository
{
    private IMapper _mapper;

    public HolidayPlanRepositoryEF(AbsanteeContext context, IMapper mapper) : base(context, mapper)
    {
        _mapper = mapper;
    }

    public bool CanInsertHolidayPeriod(long holidayPlanId, IHolidayPeriod periodDate)
    {
        return _context.Set<HolidayPlanDataModel>().Any
            (h => h.Id == holidayPlanId && h.GetHolidayPeriods().Any
                (hp => hp._periodDate._initDate <= periodDate._periodDate._initDate
                    && hp._periodDate._finalDate >= periodDate._periodDate._finalDate));
    }

    private async Task<bool> CanInsertAsync(IHolidayPlan holidayPlan)
    {
        return !await _context.Set<HolidayPlanDataModel>().AnyAsync(hp => hp.Id == holidayPlan.GetCollaboratorId());
    }


    public async Task<bool> AddHolidayPlanAsync(IHolidayPlan holidayPlan)
    {
        bool canInsert = await CanInsertAsync(holidayPlan);

        if (canInsert)
        {
            await _context.Set<HolidayPlanDataModel>().AddAsync(_mapper.Map<HolidayPlan, HolidayPlanDataModel>((HolidayPlan)holidayPlan));
        }

        return canInsert;
    }

    public async Task<IEnumerable<IHolidayPlan>> FindAllCollaboratorsWithHolidayPeriodsBetweenDatesAsync(PeriodDate periodDate)
    {
        return await _context.Set<HolidayPlanDataModel>()
            .Where(hp => hp.GetHolidayPeriods().Any(h => periodDate.Contains(h._periodDate)))
            .Select(hp => _mapper.Map<HolidayPlanDataModel,HolidayPlan>(hp))
            .ToListAsync();
    }

    public async Task<IEnumerable<IHolidayPeriod>> FindAllHolidayPeriodsForAllCollaboratorsBetweenDatesAsync(List<long> collabIds, PeriodDate periodDate)
    {
        var ret = await _context.Set<HolidayPlanDataModel>()
            .Where(hp => collabIds.Contains(hp.CollaboratorId))
            .SelectMany(hp => hp.HolidayPeriodsDM)
            .Where(hperiod => periodDate._initDate <= hperiod.PeriodDate._initDate
                     && periodDate._finalDate >= hperiod.PeriodDate._finalDate)
            .ToListAsync();

        return ret.Select(h => _mapper.Map<HolidayPeriodDataModel, HolidayPeriod>(h));
    }

    public async Task<IEnumerable<IHolidayPeriod>> FindAllHolidayPeriodsForCollaboratorBetweenDatesAsync(long collaboratorId, PeriodDate periodDate)
    {
        return await _context.Set<HolidayPlanDataModel>()
            .Where(hp => hp.CollaboratorId == collaboratorId)
            .SelectMany(hp => hp.GetHolidayPeriods())
            .Where(hperiod => periodDate._initDate >= hperiod._periodDate._initDate
                           && periodDate._finalDate <= hperiod._periodDate._finalDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<IHolidayPeriod>> FindAllHolidayPeriodsLongerThanForCollaboratorBetweenDatesAsync(long collaboratorId, PeriodDate periodDate, int days)
    {
        try
        {
            HolidayPlanDataModel? holidayPlan =
                await _context.Set<HolidayPlanDataModel>()
                              .FirstOrDefaultAsync(hp => hp.CollaboratorId == collaboratorId);

            if (holidayPlan == null)
                return new List<IHolidayPeriod>();

            List<IHolidayPeriod> result =
                holidayPlan.GetHolidayPeriods().Where(h => periodDate.Contains(h._periodDate) &&
                                                (periodDate.Duration() > days))
                                          .ToList();

            return result;
        }
        catch
        {
            throw;
        }
    }

    public async Task<IEnumerable<IHolidayPlan>> FindAllWithHolidayPeriodsLongerThanAsync(int days)
    {
        var hpDm = await _context.Set<HolidayPlanDataModel>()
            .Where(hp => hp.HolidayPeriodsDM
                .Any(h => (h.PeriodDate._finalDate.DayNumber - h.PeriodDate._initDate.DayNumber + 1) > days))
            .Include(hp => hp.HolidayPeriodsDM)
            .ToListAsync();

        return hpDm.Select(h => _mapper.Map<HolidayPlanDataModel, HolidayPlan>(h));
    }

    public async Task<IEnumerable<IHolidayPeriod>> FindHolidayPeriodsByCollaboratorAsync(long collaboratorId)
    {
        var holidayPlans = await _context.Set<HolidayPlanDataModel>()
            .FirstOrDefaultAsync(hp => hp.CollaboratorId == collaboratorId);

        if (holidayPlans == null)
            return Enumerable.Empty<IHolidayPeriod>();

        return holidayPlans.GetHolidayPeriods();
    }

    public async Task<IEnumerable<IHolidayPeriod>> FindHolidayPeriodsByCollaboratorBetweenDatesAsync(long collaboratorId, PeriodDate period)
    {
        var holidayPeriods = await _context.Set<HolidayPlanDataModel>()
            .Where(hp => hp.CollaboratorId == collaboratorId)
            .SelectMany(hp => hp.HolidayPeriodsDM
                                .Where(hp => hp.PeriodDate._initDate <= period._finalDate
                                          && period._initDate <= hp.PeriodDate._finalDate))
            .ToListAsync();

        return holidayPeriods.Select(hp => _mapper.Map<HolidayPeriodDataModel,HolidayPeriod>(hp));
    }

    public async Task<IHolidayPlan?> FindHolidayPlanByCollaboratorAsync(long collaboratorId)
    {
        var hpDm = await _context.Set<HolidayPlanDataModel>()
            .Where(hp => hp.CollaboratorId == collaboratorId)
            .Include(hp => hp.HolidayPeriodsDM)
            .SingleOrDefaultAsync();

        if (hpDm == null) return null;

        return _mapper.Map<HolidayPlanDataModel,HolidayPlan>(hpDm);
    }

    public async Task<IEnumerable<IHolidayPlan>> FindHolidayPlansWithinPeriodAsync(PeriodDate periodDate)
    {
        var holidayPlansDMs = await _context.Set<HolidayPlanDataModel>()
                    .Where(hp => hp.HolidayPeriodsDM
                        .Any(hperiod => periodDate._initDate <= hperiod.PeriodDate._initDate
                                    && periodDate._finalDate >= hperiod.PeriodDate._finalDate))
                    // include adicionado com o prof, sendo que hplan e hperiod são um agregado
                    .Include(hp => hp.HolidayPeriodsDM)
                    .ToListAsync();

        return holidayPlansDMs.Select(h => _mapper.Map<HolidayPlanDataModel, HolidayPlan>(h));
    }

    public override async Task<IHolidayPlan?> GetByIdAsync(long id)
    {
        var hpDM = await _context.Set<HolidayPlanDataModel>().FirstOrDefaultAsync(hp => hp.Id == id);

        if (hpDM == null)
            return null;

        var hp = _mapper.Map<HolidayPlanDataModel, HolidayPlan>(hpDM);
        return hp;
    }

    public override IHolidayPlan? GetById(long id)
    {
        var hpDM = _context.Set<HolidayPlanDataModel>().FirstOrDefault(hp => hp.Id == id);

        if (hpDM == null)
            return null;

        var hp = _mapper.Map<HolidayPlanDataModel, HolidayPlan>(hpDM);
        return hp;
    }
}