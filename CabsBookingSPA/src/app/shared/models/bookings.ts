export interface Bookings {
    places?: any;
    cabTypes?: any;
    id: number;
    email: string;
    bookingDate: Date;
    bookingTime?: any;
    fromPlace: number;
    toPlace: number;
    pickupAddress: string;
    landMark: string;
    pickupDate?: Date;
    pickupTime?: any;
    cabTypesId: number;
    contactNo: string;
    status: string;
}
