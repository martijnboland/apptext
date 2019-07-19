import React, { ChangeEvent } from 'react';
import { FieldProps, getIn } from 'formik';
import { ICustomFieldProps } from './ICustomFieldProps';
import classNames from 'classnames';
import { defaultProps } from 'react-select/lib/Creatable';

export interface SelectOption {
  value: string,
  label: string
}

interface SelectProps extends ICustomFieldProps {
  options: Array<SelectOption>,
  insertEmpty?: boolean,
  onChange?: (value: string) => void 
}

export const Select: React.FunctionComponent<FieldProps & SelectProps> = ({ 
  field, 
  form: { touched, errors },
  options, 
  className, 
  label, 
  insertEmpty,
  onChange
}) => {
  const cssClass = className || 'form-group';
  const error = getIn(errors, field.name);
  const touch = getIn(touched, field.name);

  const selectChanged = (ev: ChangeEvent<HTMLSelectElement>) => {
    if (onChange) {
      onChange(ev.target.value);
    }
    field.onChange(ev);
  }

  return (
    <div className={cssClass}>
      <label htmlFor={field.name}>{label}</label>
      <select {...field} className={classNames('form-control', { 'is-invalid': error })} onChange={selectChanged}>
        {insertEmpty && 
          <option key={null} />
        }
        {options.map(o => <option key={o.value} value={o.value}>{o.label}</option>)}
      </select>
      {error && touch && <div className="invalid-feedback">{error}</div>}
    </div>
  );
};