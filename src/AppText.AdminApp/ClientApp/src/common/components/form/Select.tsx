import React from 'react';
import { FieldProps } from 'formik';
import { ICustomFieldProps } from './ICustomFieldProps';
import classNames from 'classnames';

export interface SelectOption {
  value: string,
  description: string
}

interface SelectProps extends ICustomFieldProps {
  options: Array<SelectOption>,
  insertEmpty?: boolean
}

export const Select: React.FunctionComponent<FieldProps & SelectProps> = ({ 
  field, 
  form: { touched, errors },
  options, 
  className, 
  label, 
  insertEmpty 
}) => {
  const cssClass = className || 'form-group';

  return (
    <div className={cssClass}>
      <label htmlFor={field.name}>{label}</label>
      <select {...field} className={classNames('form-control', { 'is-invalid': errors[field.name] })}>
        {insertEmpty && 
          <option key={null} />
        }
        {options.map(o => <option key={o.value} value={o.value}>{o.description}</option>)}
      </select>
      {errors[field.name] && touched[field.name] && <div className="invalid-feedback">{errors[field.name]}</div>}
    </div>
  );
};