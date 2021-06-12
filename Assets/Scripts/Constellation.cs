using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constellation : MonoBehaviour
{
    // Each pair of stars represents a line (similar to what is done in 3D models with polygons)
    public List<Star> stars;

    private void Start()
    {
        this.SortConstellation(this.stars);
    }

    public bool IsSameConstellation(List<Star> _otherStars)
    {
        if (stars.Count != _otherStars.Count)
        {
            return false;
        }

        List<Star> otherStars = new List<Star>(_otherStars); // clone to not change the original list
        this.SortConstellation(otherStars);
        for (int i = 0; i< otherStars.Count; i++)
        {
            if (!stars[i].Equals(otherStars[i]))
            {
                return false;
            }
        }
        return true;
    }

    // Will sort lines and pairs of stars forming a line
    private void SortConstellation(List<Star> stars)
    {
        for (int i = 0; i < stars.Count; i += 2)
        {
            if (stars[i].GetInstanceID() > stars[i + 1].GetInstanceID())
            {
                this.SwapStars(stars, i, i + 1);
            }
        }

        for (int i = 0; i < stars.Count - 2; i += 2)
        {
            for (int j = 0; j < stars.Count - 2 - i; j += 2)
            {
                if (stars[j].GetInstanceID() > stars[j + 2].GetInstanceID())
                {
                    this.SwapLines(stars, j, j + 2);
                }
                else if (stars[j].GetInstanceID() == stars[j + 2].GetInstanceID() && stars[j + 1].GetInstanceID() > stars[j + 3].GetInstanceID())
                {
                    this.SwapLines(stars, j, j + 2);
                }
            }
        }
    }
    private void SwapStars(List<Star> stars, int i, int j)
    {
        Star tmp = stars[i];
        stars[i] = stars[j];
        stars[j] = tmp;
    }

    // i and j are the positions of the first star in the pair
    private void SwapLines(List<Star> stars, int i, int j)
    {
        this.SwapStars(stars, i, j);
        this.SwapStars(stars, i + 1, j + 1);
    }
}
