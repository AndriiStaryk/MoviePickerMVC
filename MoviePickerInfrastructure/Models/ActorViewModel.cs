﻿using MoviePickerDomain.Model;
using System.Runtime.CompilerServices;

namespace MoviePickerInfrastructure.Models;

public class ActorViewModel
{
    private MoviePickerContext _context;
    public Actor Actor { get; set; } = null!;

    public List<Movie> MoviesWithThisActor { get; set; }

    public ActorViewModel(MoviePickerContext context,Actor actor)
    {
        _context = context;
        Actor = actor;

        MoviesWithThisActor = context.MoviesActors
            .Where(ma => ma.ActorId == actor.Id)
            .Select(ma => ma.Movie).ToList()!;
    }
}
