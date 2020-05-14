import React from 'react';
import classNames from 'classnames';
import DatePicker from 'react-datepicker';
import { FieldProps, getIn } from 'formik';
import { ICustomFieldProps } from './ICustomFieldProps';

import './DateTimeInput.scss';

interface DateTimeInputProps extends ICustomFieldProps {
}

export const DateTimeInput: React.FunctionComponent<FieldProps & DateTimeInputProps> = ({
  label,
  className,
  field, 
  form, // { touched, errors }, // also values, setXXXX, handleXXXX, dirty, isValid, status, etc.
  ...rest
}) => {
  const cssClass = className || 'form-group';
  const error = getIn(form.errors, field.name);

  const dateValue = field.value ? new Date(field.value) : undefined;
  const onChange = (newDate: Date) => {
    if (newDate) {
      form.setFieldValue(field.name, newDate.toISOString());
    } else {
      form.setFieldValue(field.name, null);
    }
  }

  return (
    <div className={cssClass}>
      <label>{label}</label>
      <DatePicker 
        selected={dateValue}
        onChange={onChange}
        showTimeSelect
        dateFormat="MMMM d, yyyy HH:mm"
        timeFormat="HH:mm"
        className={classNames('form-control', { 'is-invalid': error })}
      />
      {error && <div className="invalid-feedback">{error}</div>}
    </div>
  );
};