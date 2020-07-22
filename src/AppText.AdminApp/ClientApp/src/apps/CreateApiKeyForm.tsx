import React from 'react';
import { Formik, Field, FormikHelpers } from 'formik';
import { useTranslation } from 'react-i18next';
import { TextInput } from '../common/components/form';
import { FaSave } from 'react-icons/fa';

interface ICreateApiKeyFormProps {
  onCreate(name: string): Promise<any>,
  onClose(): void
}

const CreateApiKeyForm: React.FunctionComponent<ICreateApiKeyFormProps> = ({ onCreate, onClose }) => {
  const { t } = useTranslation(['Labels']);
  
  const onSubmit = (values: any, actions: FormikHelpers<any>) => {
    onCreate(values.name)
      .then(res => {
        if (! res.ok) {
          actions.setErrors(res.errors);
        }
      });
  }
  
  return (
    <Formik
      initialValues={{
        name: ''
      }}
      onSubmit={onSubmit}
    >
      {({ handleSubmit }) => {
        return (
          <form onSubmit={handleSubmit}>
            <Field name="name" label={t('Labels:Name')} component={TextInput} />
            <button type="submit" className="btn btn-primary mr-2"><FaSave className="mr-1" />{t('Labels:CreateButton')}</button>
            <button type="button" className="btn btn-link" onClick={onClose}>{t('Labels:CancelButton')}</button>
          </form>
        )
      }}
    </Formik>
  );
};

export default CreateApiKeyForm;
