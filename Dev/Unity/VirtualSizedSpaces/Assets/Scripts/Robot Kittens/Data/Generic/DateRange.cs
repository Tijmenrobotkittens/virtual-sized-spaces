using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateRange 
{

	public DateTime _fromDate;
	public DateTime _toDate;
	public string icon;
       
    public DateRange(string from, string to)
    {
		this._fromDate = DateTime.Parse(from);
        this._toDate = DateTime.Parse(to);
	}

	public DateTime From {
		set {
			_fromDate = value;
		}
        get
		{
			return _fromDate;
		}
	}

	public DateTime To
	{
		set
		{
			_toDate = value;
		}
		get
		{
            return _toDate;
		}
	}

    public bool HasPassed(DateTime date) {
        if (date > _toDate.AddDays(1)) {
            return true;
        }
        else
        {
            return false;
        }

    }

    public bool InRange(DateTime date)
    {
        Debug.Log("fromtdate :" + _fromDate);
        Debug.Log("todate :" + _toDate);
        Debug.Log("now :" + date);

        bool ret = false;
        if (date > _fromDate && date < _toDate.AddDays(1))
        {
            ret = true;
        }
        return ret;
    }

}

